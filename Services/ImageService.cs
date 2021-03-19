#nullable enable
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Domain.Services.Communication;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace HR_Management.Services
{
    public class ImageService : IImageService
    {
        private readonly IUriService _uriService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ImageService(IUriService uriService,
            IWebHostEnvironment webHostEnvironment,
            IPersonRepository personRepository,
            IUnitOfWork unitOfWork)
        {
            this._uriService = uriService;
            this._webHostEnvironment = webHostEnvironment;
            this._personRepository = personRepository;
            this._unitOfWork = unitOfWork;
        }
        public async Task<ImageResponse<Uri>> SaveImage(int personId, Stream imageStream, bool isMobile = false)
        {
            // Validate properties of image
            if (imageStream is null || imageStream.Length == 0)
                new ImageResponse<Uri>("There is no image.");
            if (imageStream?.Length > 5242880)
                new ImageResponse<Uri>("Image cannot be bigger than 5 MB.");
            // Validate Id is existent?
            var tempPerson = await _personRepository.FindByIdAsync(personId);
            if (tempPerson is null)
                return new ImageResponse<Uri>($"Id '{personId}' is not existent.");
            // Path of image
            string webPath = string.Format($"/Images/Web/{tempPerson.StaffId}.jpg");
            string rootWebPath = string.Concat(_webHostEnvironment.WebRootPath, webPath);

            string mobilePath = string.Format($"/Images/Mobile/{tempPerson.StaffId}.jpg");
            string rootMobilePath = string.Concat(_webHostEnvironment.WebRootPath, mobilePath);
            try
            {
                bool isSuccess = Initialize(hasValue: tempPerson.Avatar, imageStream: imageStream, webPath: rootWebPath, mobilePath: rootMobilePath);
                if (!isSuccess)
                    return new ImageResponse<Uri>("Saving fault image.");
                tempPerson.Avatar = $"{tempPerson.StaffId}.jpg";
                await _unitOfWork.CompleteAsync();

                Uri tempURI = isMobile ? _uriService.GetRouteUri(mobilePath) : _uriService.GetRouteUri(webPath);
                return new ImageResponse<Uri>(tempURI);
            }
            catch (Exception ex)
            {
                return new ImageResponse<Uri>($"An error occurred when saving the Image: {ex.Message}");
            }

            return new ImageResponse<Uri>("test");
        }

        #region Initialize
        private bool Initialize(string hasValue, Stream imageStream, string webPath, string mobilePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(hasValue))
                {
                    DeletePhotoIfExist(webPath);
                    DeletePhotoIfExist(mobilePath);
                }
                bool isSuccess = SetThumbnails(imageStream, webPath: webPath, mobilePath: mobilePath);

                return isSuccess;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Set Thumbnails
        private bool SetThumbnails(Stream imageStream, string webPath, string mobilePath)
        {
            var originalImage = Image.FromStream(imageStream);

            if (!ValidateImageFormat(originalImage))
                throw new Exception();

            SetCorrectOrientation(originalImage);

            bool isSuccessWeb = CreateAndSaveThumbnail(originalImage, 500, webPath);
            bool isSuccessMobile = CreateAndSaveThumbnail(originalImage, 100, mobilePath);

            originalImage.Dispose();

            if (isSuccessWeb && isSuccessMobile)
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

        #region Create And Save Thumbnail
        private bool CreateAndSaveThumbnail(Image image, int size, string thumbnailPath)
        {
            var thumbnailSize = GetThumbnailSize(image, size);
            try
            {
                using (var bitmap = ResizeImage(image, thumbnailSize.Width, thumbnailSize.Height))
                {
                    bitmap.Save($"{thumbnailPath}", ImageFormat.Jpeg);
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
    }
}
