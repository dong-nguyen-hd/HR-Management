using Business.Communication;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Information;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
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
        private readonly IUriService _uriService;
        private readonly IPersonRepository _personRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HostResource _hostResource;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ImageService> _logger;
        #endregion

        #region Constructor
        public ImageService(IUriService uriService,
            IPersonRepository personRepository,
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment env,
            ILogger<ImageService> logger,
            IOptionsMonitor<HostResource> hostResource,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(responseMessage)
        {
            this._uriService = uriService;
            this._personRepository = personRepository;
            this._accountRepository = accountRepository;
            this._unitOfWork = unitOfWork;
            this._hostResource = hostResource.CurrentValue;
            this._env = env;
        }
        #endregion

        #region Method
        public async Task<BaseResponse<Uri>> SaveImageAccountAsync(int accountId, Stream imageStream)
        {
            // Validate properties of image
            if (imageStream is null || imageStream.Length == 0)
                return new BaseResponse<Uri>(ResponseMessage.Values["Image_NoData"]);
            if (imageStream?.Length > 5242880) // 5 MB
                return new BaseResponse<Uri>(ResponseMessage.Values["Image_Bigger_Error"]);
            // Validate Id is existent?
            var tempAccount = await _accountRepository.GetByIdAsync(accountId);
            if (tempAccount is null)
                return new BaseResponse<Uri>(ResponseMessage.Values["Account_Id_NoData"]);

            // Path of image
            string originalPath = string.Format($"{_hostResource.OriginalImagePath}{tempAccount.UserName}.jpg");
            string rootOriginalPath = string.Concat(_env.WebRootPath, originalPath);

            string thumbnailPath = string.Format($"{_hostResource.ThumbnailImagePath}{tempAccount.UserName}.jpg");
            string rootThumbnailPath = string.Concat(_env.WebRootPath, thumbnailPath);

            try
            {
                bool isSuccess = Initialize(hasValue: tempAccount.Avatar, imageStream, rootOriginalPath, rootThumbnailPath);
                if (!isSuccess)
                {
                    tempAccount.Avatar = "default.jpg";
                    return new BaseResponse<Uri>(ResponseMessage.Values["Image_Saving_Error"]);
                }

                tempAccount.Avatar = $"{tempAccount.UserName}.jpg";
                await _unitOfWork.CompleteAsync();

                return new BaseResponse<Uri>(_uriService.GetRouteUri(thumbnailPath));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ResponseMessage.Values["Image_Saving_Error"]);
                return new BaseResponse<Uri>(ResponseMessage.Values["Image_Saving_Error"]);
            }
        }

        public async Task<BaseResponse<Uri>> SaveImagePersonAsync(int personId, Stream imageStream)
        {
            // Validate properties of image
            if (imageStream is null || imageStream.Length == 0)
                return new BaseResponse<Uri>(ResponseMessage.Values["Image_NoData"]);
            if (imageStream?.Length > 5242880) // 5 MB
                return new BaseResponse<Uri>(ResponseMessage.Values["Image_Bigger_Error"]);
            // Validate Id is existent?
            var tempPerson = await _personRepository.GetByIdAsync(personId);
            if (tempPerson is null)
                return new BaseResponse<Uri>(ResponseMessage.Values["Person_Id_NoData"]);

            // Path of image
            string originalPath = string.Format($"{_hostResource.OriginalImagePath}{tempPerson.StaffId}.jpg");
            string rootOriginalPath = string.Concat(_env.WebRootPath, originalPath);

            string thumbnailPath = string.Format($"{_hostResource.ThumbnailImagePath}{tempPerson.StaffId}.jpg");
            string rootThumbnailPath = string.Concat(_env.WebRootPath, thumbnailPath);

            try
            {
                bool isSuccess = Initialize(hasValue: tempPerson.Avatar, imageStream, rootOriginalPath, rootThumbnailPath);
                if (!isSuccess)
                {
                    tempPerson.Avatar = "default.jpg";
                    return new BaseResponse<Uri>(ResponseMessage.Values["Image_Saving_Error"]);
                }

                tempPerson.Avatar = $"{tempPerson.StaffId}.jpg";
                await _unitOfWork.CompleteAsync();

                return new BaseResponse<Uri>(_uriService.GetRouteUri(thumbnailPath));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ResponseMessage.Values["Image_Saving_Error"]);
                return new BaseResponse<Uri>(ResponseMessage.Values["Image_Saving_Error"]);
            }
        }

        #region Initialize
        private bool Initialize(string hasValue, Stream imageStream, string originalPath, string thumbnailPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(hasValue))
                {
                    DeletePhotoIfExist(originalPath);
                    DeletePhotoIfExist(thumbnailPath);
                }
                bool isSuccess = SetThumbnails(imageStream, originalPath, thumbnailPath);

                return isSuccess;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Set Thumbnails
        private bool SetThumbnails(Stream imageStream, string originalPath, string thumbnailPath)
        {
            var rawImage = Image.FromStream(imageStream);

            if (!ValidateImageFormat(rawImage))
                throw new Exception();

            SetCorrectOrientation(rawImage);

            bool isSuccessOriginal = CreateAndSaveImage(rawImage, 500, originalPath);
            bool isSuccessThumbnail = CreateAndSaveImage(rawImage, 100, thumbnailPath);

            rawImage.Dispose();

            if (isSuccessOriginal && isSuccessThumbnail)
                return true;

            return false;
        }
        #endregion

        #region Validate Image Format
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

        #region Set Correct Orientation
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
        #endregion

        #region Create And Save Image
        private bool CreateAndSaveImage(Image image, int size, string imagePath)
        {
            var thumbnailSize = GetThumbnailSize(image, size);
            try
            {
                using (var bitmap = ResizeImage(image, thumbnailSize.Width, thumbnailSize.Height))
                {
                    bitmap.Save($"{imagePath}", ImageFormat.Jpeg);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Get Thumbnail Size
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
        #endregion

        #region Resize Image
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
        #endregion

        #region Delete Photo If Exist
        public void DeletePhotoIfExist(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        #endregion

        #endregion
    }
}
