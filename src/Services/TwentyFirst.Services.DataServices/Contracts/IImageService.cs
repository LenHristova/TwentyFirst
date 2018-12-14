namespace TwentyFirst.Services.DataServices.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Models.Images;

    public interface IImageService
    {
        /// <summary>
        /// Upload images async and return count of the corrupted images
        /// that was not uploaded.
        /// </summary>
        /// <param name="imagesCreateInputModel"></param>
        /// <param name="creatorId"></param>
        /// <returns></returns>
        Task<int> UploadAsync(ImagesCreateInputModel imagesCreateInputModel, string creatorId);

        /// <summary>
        /// Search if the description contains search term, and returns that images
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        IEnumerable<TModel> GetBySearchTerm<TModel>(string searchTerm);
    }
}
