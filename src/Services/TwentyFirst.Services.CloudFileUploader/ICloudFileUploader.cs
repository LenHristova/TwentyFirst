namespace TwentyFirst.Services.CloudFileUploader
{
    using Microsoft.AspNetCore.Http;

    public interface ICloudFileUploader
    {
        FileUrls UploadImageAsync(IFormFile image);
    }
}
