using AutoMapper;
using Business.Communication;
using Business.CustomException;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Account;
using Business.Resources.Information;
using Business.Resources.Person;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ImageCrossPlatformService : ResponseMessageService, IImageService
    {
        #region Property
        private readonly IPersonRepository _personRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HostResource _hostResource;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public ImageCrossPlatformService(IPersonRepository personRepository,
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment env,
            IMapper mapper,
            IOptionsMonitor<HostResource> hostResource,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(responseMessage)
        {
            this._personRepository = personRepository;
            this._accountRepository = accountRepository;
            this._unitOfWork = unitOfWork;
            this._hostResource = hostResource.CurrentValue;
            this._env = env;
            this._mapper = mapper;
        }
        #endregion

        #region Method
        public async Task<BaseResponse<AccountResource>> SaveImageAccountAsync(int accountId, Stream imageStream)
        {
            try
            {
                IImageFormat imageFormat;
                using (var rawImage = Image.Load(imageStream, out imageFormat))
                {
                    // Validate properties of image
                    var validateImage = ValidateImage<AccountResource>(imageStream, imageFormat);
                    if (!validateImage.isSuccess)
                        return validateImage.result;

                    // Validate Id is existent?
                    var tempAccount = await _accountRepository.GetByIdAsync(accountId);
                    if (tempAccount is null)
                        return new BaseResponse<AccountResource>(ResponseMessage.Values["Account_Id_NoData"]);

                    string avatarFileName = CreateFileName(tempAccount.UserName, imageFormat.FileExtensions.FirstOrDefault());
                    tempAccount.Avatar = await Initialize(tempAccount.Avatar, avatarFileName, rawImage);

                    await _unitOfWork.CompleteAsync();

                    return new BaseResponse<AccountResource>(_mapper.Map<AccountResource>(tempAccount));
                }
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Image_Saving_Error"], ex);
            }
        }

        public async Task<BaseResponse<PersonResource>> SaveImagePersonAsync(int personId, Stream imageStream)
        {
            try
            {
                IImageFormat imageFormat;
                using (var rawImage = Image.Load(imageStream, out imageFormat))
                {
                    // Validate properties of image
                    var validateImage = ValidateImage<PersonResource>(imageStream, imageFormat);
                    if (!validateImage.isSuccess)
                        return validateImage.result;

                    // Validate Id is existent?
                    var tempPerson = await _personRepository.GetByIdAsync(personId);
                    if (tempPerson is null)
                        return new BaseResponse<PersonResource>(ResponseMessage.Values["Person_Id_NoData"]);

                    string avatarFileName = CreateFileName(tempPerson.StaffId, imageFormat.FileExtensions.FirstOrDefault());
                    tempPerson.Avatar = await Initialize(tempPerson.Avatar, avatarFileName, rawImage);

                    await _unitOfWork.CompleteAsync();

                    return new BaseResponse<PersonResource>(_mapper.Map<PersonResource>(tempPerson));
                }
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Image_Saving_Error"], ex);
            }
        }

        #region Private work
        private static string CreateFileName(string avatarName, string fileExtension)
        {
            fileExtension ??= "jpg";

            return $"{avatarName}-{Guid.NewGuid()}.{fileExtension}";
        }

        private static bool CheckExistentAvatar(string avatarName)
            => (string.IsNullOrEmpty(avatarName) || avatarName.Equals("default.jpg")) ? false : true;

        private async Task<string> Initialize(string oldFileName, string newFileName, Image rawImage)
        {
            string defaultName = "default.jpg";

            try
            {
                bool isExistent = CheckExistentAvatar(oldFileName);
                var path = GetRootPath(newFileName);
                var tempPath = GetTemporaryPath(oldFileName);

                // Change file name if it's existent
                if (isExistent)
                {
                    bool isSuccesschangefilename = false;
                    isSuccesschangefilename = ChangeFileName(path.originalPath, tempPath.originalPath);
                    isSuccesschangefilename = ChangeFileName(path.thumbnailPath, tempPath.thumbnailPath);

                    if (!isSuccesschangefilename)
                        return defaultName;
                }

                bool isSuccess = await SetThumbnailsAsync(rawImage, path.originalPath, path.thumbnailPath);

                if (isSuccess && isExistent)
                {
                    DeletePhotoIfExist(tempPath.originalPath);
                    DeletePhotoIfExist(tempPath.thumbnailPath);
                }
                else if (isExistent)
                {
                    ChangeFileName(tempPath.originalPath, path.originalPath);
                    ChangeFileName(tempPath.thumbnailPath, path.thumbnailPath);

                    return oldFileName;
                }

                return newFileName;
            }
            catch
            {
                return defaultName;
            }
        }

        private (string thumbnailPath, string originalPath) GetRootPath(string avatarFileName)
        {
            string originalPath = string.Format($"{_hostResource.OriginalImagePath}{avatarFileName}");
            string rootOriginalPath = string.Concat(_env.WebRootPath, originalPath);

            string thumbnailPath = string.Format($"{_hostResource.ThumbnailImagePath}{avatarFileName}");
            string rootThumbnailPath = string.Concat(_env.WebRootPath, thumbnailPath);

            return (rootThumbnailPath, rootOriginalPath);
        }

        private (string thumbnailPath, string originalPath) GetTemporaryPath(string avatarFileName)
        {
            var guid = Guid.NewGuid().ToString();

            string originalPath = string.Format($"{_hostResource.OriginalImagePath}{guid}{avatarFileName}");
            string rootOriginalPath = string.Concat(_env.WebRootPath, originalPath);

            string thumbnailPath = string.Format($"{_hostResource.ThumbnailImagePath}{guid}{avatarFileName}");
            string rootThumbnailPath = string.Concat(_env.WebRootPath, thumbnailPath);

            return (rootThumbnailPath, rootOriginalPath);
        }

        private bool DeletePhotoIfExist(string path)
        {
            if (!File.Exists(path))
                return false;

            File.Delete(path);

            return true;
        }

        private bool ChangeFileName(string sourceFileName, string destFileName)
        {
            if (!File.Exists(sourceFileName) || File.Exists(destFileName))
                return false;

            File.Move(sourceFileName, destFileName);

            return true;
        }

        private async Task<bool> SetThumbnailsAsync(Image rawImage, string originalPath, string thumbnailPath)
        {
            try
            {
                var taskOne = CreateAndSaveImageAsync(rawImage, 500, originalPath);
                var taskTwo = CreateAndSaveImageAsync(rawImage, 100, thumbnailPath);

                await Task.WhenAll(taskOne, taskTwo);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static async Task CreateAndSaveImageAsync(Image rawImage, int size, string path)
        {
            var avatarSize = GetThumbnailSize(rawImage, size);

            rawImage.Mutate(x => x.AutoOrient().Resize(avatarSize));

            await rawImage.SaveAsync(path);
        }

        private static Size GetThumbnailSize(Image original, int size)
        {
            var originalWidth = original.Width;
            var originalHeight = original.Height;

            double factor;
            if (originalWidth > originalHeight)
            {
                factor = (double)size / originalWidth;
            }
            else
            {
                factor = (double)size / originalHeight;
            }

            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }

        private (bool isSuccess, BaseResponse<T> result) ValidateImage<T>(Stream imageStream, IImageFormat imageFormat)
        {
            List<string> messages = new();

            // Validate properties of image
            if (imageStream is null || imageStream.Length == 0)
                messages.Add(ResponseMessage.Values["Image_NoData"]);
            if (imageStream?.Length > 5242880) // 5 MB
                messages.Add(ResponseMessage.Values["Image_Bigger_Error"]);
            if (!ValidateImageFormat(imageFormat))
                messages.Add(ResponseMessage.Values["Image_Format_Error"]);

            return messages.Count == 0 ? (true, null) : (false, new BaseResponse<T>(messages));
        }

        private static bool ValidateImageFormat(IImageFormat imageFormat)
        {
            string nameFormat = imageFormat.Name.ToUpper();

            if (nameFormat.Equals("JPEG"))
                return true;

            if (nameFormat.Equals("PNG"))
                return true;

            if (nameFormat.Equals("GIF"))
                return true;

            if (nameFormat.Equals("BMP"))
                return true;

            return false;
        }
        #endregion

        #endregion
    }
}
