namespace TwentyFirst.Services.DataServices.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Models.Images;
    using Data.Models;

    public interface IImageService
    {
        /// <summary>
        /// Search if the description contains search term, and returns that images
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        Task<IEnumerable<TModel>> GetBySearchTermAsync<TModel>(string searchTerm);

        Task<IEnumerable<TModel>> GetBySearchTermWithDeletedAsync<TModel>(string searchTerm);

        Task DeleteAsync(string id);

        Task RecoverAsync(string id);

        Task CreateAsync(
            ImagesCreateInputModel imagesCreateInputModel,
            string creatorId,
            string url,
            string thumbUrl);

        /// <summary>
        /// Gets image by id.
        /// Throw InvalidImageIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidImageException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Image> GetAsync(string id);

        /// <summary>
        /// Gets marked as deleted image by id.
        /// Throw InvalidImageIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidImageException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Image> GetDeletedAsync(string id);
    }
}
