namespace TwentyFirst.Services.DataServices.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Models.Categories;
    using Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICategoryService
    { 
        Task<IEnumerable<TModel>> AllWithArchived<TModel>();

        Task<Category> CreateAsync(CategoryCreateInputModel categoryCreateInputModel);

        Task<Category> EditAsync(CategoryUpdateInputModel categoryUpdateInputModel);

        Task<Category> RecoverAsync(string id);

        /// <summary>
        /// Get category by id and project it to given model.
        /// Throw InvalidCategoryIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidCategoryIdException"></exception>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TModel> GetAsync<TModel>(string id);

        /// <summary>
        /// Get category by id
        /// Throw InvalidCategoryIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidCategoryIdException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Category> GetAsync(string id);

        Task<Category> ArchiveAsync(string id);

        /// <summary>
        /// Get archived category by id and project it to given model.
        /// Throw InvalidCategoryIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidCategoryIdException"></exception>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TModel> GetArchivedAsync<TModel>(string id);

        /// <summary>
        /// Get archived category by id.
        /// Throw InvalidCategoryIdException if id is not present.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Category> GetArchivedAsync(string id);

        Task<IEnumerable<TModel>> All<TModel>();

        Task<IEnumerable<SelectListItem>> AllToSelectListItemsAsync();
    }
}
