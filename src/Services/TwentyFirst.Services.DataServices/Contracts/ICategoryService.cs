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

        Task<bool> CreateAsync(CategoryCreateInputModel categoryCreateInputModel);

        Task<bool> EditAsync(CategoryUpdateInputModel categoryUpdateInputModel);

        Task<bool> ArchiveAsync(string id);

        Task<bool> RecoverAsync(string id);

        bool Exists(string id);

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

        Task<IEnumerable<SelectListItem>> AllToSelectListItemsAsync();

        Task<TModel> GetArchivedAsync<TModel>(string id);
    }
}
