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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ImageService : ResponseMessageService, IImageService
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
        public ImageService(IPersonRepository personRepository,
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
                // Validate properties of image
                var validateImage = ValidateImage<AccountResource>(imageStream);
                if (!validateImage.isSuccess)
                    return validateImage.result;

                // Validate Id is existent?
                var tempAccount = await _accountRepository.GetByIdAsync(accountId);
                if (tempAccount is null)
                    return new BaseResponse<AccountResource>(ResponseMessage.Values["Account_Id_NoData"]);

                // Path of image
                var path = GetRootPath(tempAccount.UserName);

                tempAccount.Avatar = Initialize(CheckExistentAvatar(tempAccount.Avatar), tempAccount.UserName, imageStream);

                await _unitOfWork.CompleteAsync();

                return new BaseResponse<AccountResource>(_mapper.Map<AccountResource>(tempAccount));
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
                // Validate properties of image
                var validateImage = ValidateImage<PersonResource>(imageStream);
                if (!validateImage.isSuccess)
                    return validateImage.result;

                // Validate Id is existent?
                var tempPerson = await _personRepository.GetByIdAsync(personId);
                if (tempPerson is null)
                    return new BaseResponse<PersonResource>(ResponseMessage.Values["Person_Id_NoData"]);

                // Path of image
                var path = GetRootPath(tempPerson.StaffId);

                tempPerson.Avatar = Initialize(CheckExistentAvatar(tempPerson.Avatar), tempPerson.StaffId, imageStream);

                await _unitOfWork.CompleteAsync();

                return new BaseResponse<PersonResource>(_mapper.Map<PersonResource>(tempPerson));
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Image_Saving_Error"], ex);
            }
        }

        #region Private work
        private static bool CheckExistentAvatar(string avatarName)
            => (string.IsNullOrEmpty(avatarName) || avatarName.Equals("default.jpg")) ? false : true;

        private string Initialize(bool isExistent, string avatarName, Stream imageStream)
        {
            string defaultName = "default.jpg";
            string originalName = string.Format($"{avatarName}.jpg");

            try
            {
                var path = GetRootPath(avatarName);
                var tempPath = GetTemporaryPath();

                // Change file name if it's existent
                if (isExistent)
                {
                    bool isSuccessChangeFileName = false;
                    isSuccessChangeFileName = ChangeFileName(path.originalPath, tempPath.originalPath);
                    isSuccessChangeFileName = ChangeFileName(path.thumbnailPath, tempPath.thumbnailPath);

                    if (!isSuccessChangeFileName)
                        return defaultName;
                }

                bool isSuccess = SetThumbnails(imageStream, path.originalPath, path.thumbnailPath);

                if (isSuccess)
                {
                    DeletePhotoIfExist(tempPath.originalPath);
                    DeletePhotoIfExist(tempPath.thumbnailPath);

                    return originalName;
                }
                else if (isExistent)
                {
                    ChangeFileName(tempPath.originalPath, path.originalPath);
                    ChangeFileName(tempPath.thumbnailPath, path.thumbnailPath);

                    return originalName;
                }

                return defaultName;
            }
            catch
            {
                return defaultName;
            }
        }

        private (string thumbnailPath, string originalPath) GetRootPath(string avatarName)
        {
            string originalPath = string.Format($"{_hostResource.OriginalImagePath}{avatarName}.jpg");
            string rootOriginalPath = string.Concat(_env.WebRootPath, originalPath);

            string thumbnailPath = string.Format($"{_hostResource.ThumbnailImagePath}{avatarName}.jpg");
            string rootThumbnailPath = string.Concat(_env.WebRootPath, thumbnailPath);

            return (rootThumbnailPath, rootOriginalPath);
        }

        private (string thumbnailPath, string originalPath) GetTemporaryPath()
        {
            var guid = Guid.NewGuid().ToString();

            string originalPath = string.Format($"{_hostResource.OriginalImagePath}{guid}.jpg");
            string rootOriginalPath = string.Concat(_env.WebRootPath, originalPath);

            string thumbnailPath = string.Format($"{_hostResource.ThumbnailImagePath}{guid}.jpg");
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

        private bool SetThumbnails(Stream imageStream, string originalPath, string thumbnailPath)
        {
            try
            {
                using (var rawImage = Image.FromStream(imageStream))
                {
                    // Set Correct Orientation
                    SetCorrectOrientation(rawImage);

                    CreateAndSaveImage(rawImage, 500, originalPath);
                    CreateAndSaveImage(rawImage, 100, thumbnailPath);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void SetCorrectOrientation(Image image)
        {
            // Property Id = 274 describe EXIF orientation parameter
            if (Array.IndexOf(image.PropertyIdList, 274) > -1)
            {
                var orientation = (int)image.GetPropertyItem(274).Value[0];
                switch (orientation)
                {
                    case 1:
                        // No rotation required.
                        break;
                    case 2:
                        image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case 3:
                        image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 4:
                        image.RotateFlip(RotateFlipType.Rotate180FlipX);
                        break;
                    case 5:
                        image.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case 6:
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 7:
                        image.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case 8:
                        image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }
                // This EXIF data is now invalid and should be removed.
                image.RemovePropertyItem(274);
            }
        }

        private void CreateAndSaveImage(Image image, int size, string imagePath)
        {
            var thumbnailSize = GetThumbnailSize(image, size);

            using (var bitmap = ResizeImage(image, thumbnailSize.Width, thumbnailSize.Height))
            {
                bitmap.Save($"{imagePath}", ImageFormat.Jpeg);
            }
        }

        private static Size GetThumbnailSize(Image original, int size = 500)
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

        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var result = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            return result;
        }

        private (bool isSuccess, BaseResponse<T> result) ValidateImage<T>(Stream imageStream)
        {
            List<string> messages = new();

            // Validate properties of image
            if (imageStream is null || imageStream.Length == 0)
                messages.Add(ResponseMessage.Values["Image_NoData"]);
            if (imageStream?.Length > 5242880) // 5 MB
                messages.Add(ResponseMessage.Values["Image_Bigger_Error"]);

            using (var rawImage = Image.FromStream(imageStream))
            {
                if (!ValidateImageFormat(rawImage))
                    messages.Add(ResponseMessage.Values["Image_Format_Error"]);
            }

            return messages.Count == 0 ? (true, null) : (false, new BaseResponse<T>(messages));
        }

        private static bool ValidateImageFormat(Image image)
        {
            if (ImageFormat.Jpeg.Equals(image.RawFormat))
                return true;

            if (ImageFormat.Png.Equals(image.RawFormat))
                return true;

            if (ImageFormat.Gif.Equals(image.RawFormat))
                return true;

            if (ImageFormat.Bmp.Equals(image.RawFormat))
                return true;

            return false;
        }
        #endregion

        #endregion
    }
}
