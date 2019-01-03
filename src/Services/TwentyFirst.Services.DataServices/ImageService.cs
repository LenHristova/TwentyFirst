namespace TwentyFirst.Services.DataServices
{
    using AutoMapper;
    using Common;
    using Common.Exceptions;
    using Common.Mapping;
    using Common.Models.Images;
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ImageService : IImageService
    {
        private readonly TwentyFirstDbContext db;

        public ImageService(TwentyFirstDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<TModel>> GetBySearchTermAsync<TModel>(string searchTerm)
        {
            searchTerm = searchTerm ?? string.Empty;

            return await this.db.Images
                .Where(i => !i.IsDeleted && i.Description.ToLower().Contains(searchTerm.ToLower().Trim()))
                .OrderByDescending(a => a.CreatedOn)
                .To<TModel>()
                .ToListAsync();
        }

        public async Task<IEnumerable<TModel>> GetBySearchTermWithDeletedAsync<TModel>(string searchTerm)
        {
            searchTerm = searchTerm ?? string.Empty;

            return await this.db.Images
                .Where(i => i.Description.ToLower().Contains(searchTerm.ToLower().Trim()))
                .OrderByDescending(a => a.CreatedOn)
                .To<TModel>()
                .ToListAsync();
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
        /// Gets image by id.
        /// Throw InvalidImageIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidImageException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TModel> GetAsync<TModel>(string id)
        {
            var image = await this.db.Images
                .Where(c => c.Id == id && !c.IsDeleted)
                .To<TModel>()
                .SingleOrDefaultAsync();

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

        /// <inheritdoc />
        /// <summary>
        /// Gets marked as deleted image by id.
        /// Throw InvalidImageIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidImageException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TModel> GetDeletedAsync<TModel>(string id)
        {
            var image = await this.db.Images
                .Where(c => c.Id == id && c.IsDeleted)
                .To<TModel>()
                .SingleOrDefaultAsync();

            CoreValidator.ThrowIfNull(image, new InvalidImageException());
            return image;
        }

        public async Task CreateAsync(
            ImagesCreateInputModel imagesCreateInputModel,
            string creatorId,
            string url,
            string thumbUrl)
        {
            var imageToDb = Mapper.Map<Image>(imagesCreateInputModel);
            imageToDb.Url = url;
            imageToDb.ThumbUrl = thumbUrl;
            imageToDb.CreatorId = creatorId;
            imageToDb.CreatedOn = DateTime.UtcNow;
            imageToDb.IsDeleted = false;

            await this.db.Images.AddAsync(imageToDb);
            await this.db.SaveChangesAsync();
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
    }
}
