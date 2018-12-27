namespace TwentyFirst.Services.CloudFileUploader
{
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    public class CloudinaryFileUploader : ICloudFileUploader
    {
        public CloudinaryFileUploader(IOptions<CloudFileUploaderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public CloudFileUploaderOptions Options { get; }

        public FileUrls UploadImageAsync(IFormFile image)
        {
            return Execute(Options.CloudName, Options.ApiKey, Options.ApiSecret, image);
        }

        public FileUrls Execute(string cloudName, string apiKey, string apiSecret, IFormFile image)
        {
            var account = new Account(cloudName, apiKey, apiSecret);
            var cloudinary = new Cloudinary(account);

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, image.OpenReadStream()),
            };

            var uploadResult = cloudinary.Upload(uploadParams);

            if (uploadResult.Error != null)
            {
                return null;
            }

            var thumbUrl = cloudinary.Api.UrlImgUp
                .Transform(new Transformation().Height(200).Crop("scale")).Secure(true)
                .BuildUrl(uploadResult.PublicId);

            var fileUrls = new FileUrls
            {
                Url = uploadResult.SecureUri.AbsoluteUri,
                ThumbUrl = thumbUrl
            };

            return fileUrls;
        }
    }
}
