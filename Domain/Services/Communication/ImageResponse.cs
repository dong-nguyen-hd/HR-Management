namespace HR_Management.Domain.Services.Communication
{
    public class ImageResponse<T> : BaseResponse<T>
    {
        public ImageResponse(T image) : base(image) { }

        public ImageResponse(string message) : base(message) { }
    }
}
