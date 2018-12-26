namespace TwentyFirst.Services.DataServices
{
    using AutoMapper;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Common.Models.Images;
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Common.Exceptions;
    using Common.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ImageService : IImageService
    {
        private readonly TwentyFirstDbContext db;
        private readonly Cloudinary cloudinary;

        public ImageService(TwentyFirstDbContext db, Cloudinary cloudinary)
        {
            this.db = db;
            this.cloudinary = cloudinary;
        }

        public async Task<int> UploadAsync(ImagesCreateInputModel imagesCreateInputModel, string creatorId)
        {
            var corruptedImages = 0;
            var images = imagesCreateInputModel.Images.ToList();

            foreach (var image in images)
            {
                var isEmpty = image.Length == 0;
                if (isEmpty)
                {
                    corruptedImages++;
                    continue;
                }

                var uploadResult = this.TryUploadToCloudinary(image);
                if (uploadResult.Error != null)
                {
                    corruptedImages++;
                    continue;
                }

                await this.SaveToDatabaseAsync(imagesCreateInputModel, creatorId, uploadResult);
            }

            return corruptedImages;
        }

        public async Task<IEnumerable<TModel>> GetBySearchTermAsync<TModel>(string searchTerm)
        {
            //TODO Extend search logic 
            //TODO Add filtering
            return await this.db.Images
                .Where(i => i.Description.ToLower().Contains(searchTerm.ToLower().Trim()) && !i.IsDeleted)
                .OrderByDescending(a => a.CreatedOn)
                .To<TModel>()
                .ToListAsync();
        }

        public async Task<IEnumerable<TModel>> GetBySearchTermWithDeletedAsync<TModel>(string searchTerm)
        {
            //TODO Extend search logic 
            //TODO Add filtering
            return await this.db.Images
                .Where(i => i.Description.ToLower().Contains(searchTerm.ToLower().Trim()))
                .OrderByDescending(a => a.CreatedOn)
                .To<TModel>()
                .ToListAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var image = await this.GetAsync(id);
            image.IsDeleted = true;
            await this.db.SaveChangesAsync();
        }

        public async Task RecoverAsync(string id)
        {
            var image = await this.GetDeletedAsync(id);
            image.IsDeleted = false;
            await this.db.SaveChangesAsync();
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets image by id.
        /// Throw InvalidImageIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidImageException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Image> GetAsync(string id)
        {
            var image = await this.db.Images
                .SingleOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            CoreValidator.ThrowIfNull(image, new InvalidImageException());
            return image;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets marked as deleted image by id.
        /// Throw InvalidImageIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidImageException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Image> GetDeletedAsync(string id)
        {
            var image = await this.db.Images
                .SingleOrDefaultAsync(c => c.Id == id && c.IsDeleted);

            CoreValidator.ThrowIfNull(image, new InvalidImageException());
            return image;
        }

        private async Task SaveToDatabaseAsync(
            ImagesCreateInputModel imagesCreateInputModel,
            string creatorId,
            ImageUploadResult uploadResult)
        {
            var thumbUrl = cloudinary.Api.UrlImgUp
                .Transform(new Transformation().Height(200).Crop("scale")).Secure(true)
                .BuildUrl(uploadResult.PublicId);

            var imageToDb = Mapper.Map<Image>(imagesCreateInputModel);
            imageToDb.Url = uploadResult.SecureUri.AbsoluteUri;
            imageToDb.ThumbUrl = thumbUrl;
            imageToDb.CreatorId = creatorId;
            imageToDb.CreatedOn = DateTime.UtcNow;
            imageToDb.IsDeleted = false;

            await this.db.Images.AddAsync(imageToDb);
            await this.db.SaveChangesAsync();

            ////TODO think about image's info needed
            //var imageToDb = new Image
            //{
            //    Title = imagesCreateInputModel.Title,
            //    Bytes = (int)result.Length,
            //    CreatedAt = DateTime.UtcNow,
            //    Format = result.Format,
            //    Height = result.Height,
            //    Path = result.Uri.AbsolutePath,
            //    PublicId = result.PublicId,
            //    ResourceType = result.ResourceType,
            //    SecureUrl = result.SecureUri.AbsoluteUri,
            //    Signature = result.Signature,
            //    Type = result.JsonObj["type"].ToString(),
            //    Url = result.Uri.AbsoluteUri,
            //    Version = int.Parse(result.Version),
            //    Width = result.Width,
            //};
        }

        private ImageUploadResult TryUploadToCloudinary(IFormFile image)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, image.OpenReadStream()),
            };

            var uploadResult = cloudinary.Upload(uploadParams);

            return uploadResult;
        }
    }
}
