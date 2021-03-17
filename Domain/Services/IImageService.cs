using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface IImageService
    {
        Task<string> SaveImage(int personId, Stream imageStream);
        string[] SetThumbnails(Stream imageStream);
        bool ValidateImageFormat(Image image);
        void SetCorrectOrientation(Image image);
        void CreateAndSaveThumbnail(Image image, int size, string thumbnailPath);
        Size GetThumbnailSize(Image original, int size = 500);
        Bitmap ResizeImage(Image image, int width, int height);
        void DeletePhotoIfExist(string photoPath);
    }
}
