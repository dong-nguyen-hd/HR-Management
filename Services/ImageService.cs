//using HR_Management.Domain.Models;
//using HR_Management.Domain.Services;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//namespace HR_Management.Services
//{
//    public class ImageService : IImageService
//    {
//        public async Task<string> SaveImage(int personId, Stream imageStream)
//        {
//            if (imageStream == null || imageStream.Length == 0)
//                throw new ServiceException(ServiceErrorCodes.InvalidPhoto, "There is no photo.");
            

//            if (imageStream.Length > 5120000)
//                throw new ServiceException(ServiceErrorCodes.InvalidPhoto, "Photo cannot be bigger than 5120000 bytes.");

//            string[] photos;

//            try
//            {
//                photos = SetThumbnails(imageStream);
//            }
//            catch (ArgumentException)
//            {
//                throw new ServiceException(ServiceErrorCodes.InvalidPhoto, "Invalid file format.");
//            }

//            if (photos != null)
//            {
//                bool userHasThumbnail = user.WebThumbnail != null;
//                var oldPhoto = user.WebThumbnail;
//                var oldThumbnail = user.Thumbnail;
//                if (userHasThumbnail)
//                {
//                    DeletePhotoIfExist(oldPhoto);
//                    DeletePhotoIfExist(oldThumbnail);
//                }
//            }
//            return user.Thumbnail;
//        }

//        public string[] SetThumbnails(Stream imageStream)
//        {
//            var originalImage = Image.FromStream(imageStream);

//            if (!ValidateImageFormat(originalImage))
//            {
//                throw new ArgumentException();
//            }

//            SetCorrectOrientation(originalImage);

//            var photoId = Guid.NewGuid().ToString();

//            var mobileThumbnail = $"Content/Photos/Users/Thumbnails/500/{photoId}.jpg";
//            var webThumbnail = $"Content/Photos/Users/Thumbnails/100/{photoId}.jpg";
//            CreateAndSaveThumbnail(originalImage, 500, mobileThumbnail);
//            CreateAndSaveThumbnail(originalImage, 100, webThumbnail);
//            originalImage.Dispose();

//            return new[] { webThumbnail, mobileThumbnail };
//        }

//        private static bool ValidateImageFormat(Image image)
//        {
//            if (ImageFormat.Jpeg.Equals(image.RawFormat))
//            {
//                return true;
//            }
//            if (ImageFormat.Png.Equals(image.RawFormat))
//            {
//                return true;
//            }
//            if (ImageFormat.Gif.Equals(image.RawFormat))
//            {
//                return true;
//            }
//            if (ImageFormat.Bmp.Equals(image.RawFormat))
//            {
//                return true;
//            }
//            return false;
//        }

//        private static void SetCorrectOrientation(Image image)
//        {
//            //property id = 274 describe EXIF orientation parameter
//            if (Array.IndexOf(image.PropertyIdList, 274) > -1)
//            {
//                var orientation = (int)image.GetPropertyItem(274).Value[0];
//                switch (orientation)
//                {
//                    case 1:
//                        // No rotation required.
//                        break;
//                    case 2:
//                        image.RotateFlip(RotateFlipType.RotateNoneFlipX);
//                        break;
//                    case 3:
//                        image.RotateFlip(RotateFlipType.Rotate180FlipNone);
//                        break;
//                    case 4:
//                        image.RotateFlip(RotateFlipType.Rotate180FlipX);
//                        break;
//                    case 5:
//                        image.RotateFlip(RotateFlipType.Rotate90FlipX);
//                        break;
//                    case 6:
//                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
//                        break;
//                    case 7:
//                        image.RotateFlip(RotateFlipType.Rotate270FlipX);
//                        break;
//                    case 8:
//                        image.RotateFlip(RotateFlipType.Rotate270FlipNone);
//                        break;
//                }
//                // This EXIF data is now invalid and should be removed.
//                image.RemovePropertyItem(274);
//            }
//        }

//        private void CreateAndSaveThumbnail(Image image, int size, string thumbnailPath)
//        {
//            var thumbnailSize = GetThumbnailSize(image, size);

//            using (var bitmap = ResizeImage(image, thumbnailSize.Width, thumbnailSize.Height))
//            {
//                bitmap.Save(_serverPath.ServerPath + thumbnailPath, ImageFormat.Jpeg);
//            }
//        }

//        private static Size GetThumbnailSize(Image original, int size = 500)
//        {
//            var originalWidth = original.Width;
//            var originalHeight = original.Height;

//            double factor;
//            if (originalWidth > originalHeight)
//            {
//                factor = (double)size / originalWidth;
//            }
//            else
//            {
//                factor = (double)size / originalHeight;
//            }

//            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
//        }

//        private Bitmap ResizeImage(Image image, int width, int height)
//        {
//            var result = new Bitmap(width, height);

//            using (var graphics = Graphics.FromImage(result))
//            {
//                graphics.CompositingQuality = CompositingQuality.HighQuality;
//                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
//                graphics.SmoothingMode = SmoothingMode.HighQuality;

//                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
//            }

//            return result;
//        }

//        public void DeletePhotoIfExist(string photoPath)
//        {
//            if (photoPath == null)
//                throw new ArgumentNullException(nameof(photoPath));

//            if (File.Exists(Path.Combine(_serverPath.ServerPath, photoPath)))
//            {
//                File.Delete(Path.Combine(_serverPath.ServerPath, photoPath));
//            }
//        }
//    }
//}
