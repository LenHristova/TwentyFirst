namespace TwentyFirst.Services.DataServices.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Models.Categories;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICategoryService
    { 
        Task<IEnumerable<TModel>> AllWithArchived<TModel>();

        Task<bool> CreateAsync(CategoryCreateInputModel categoryCreateInputModel);

        Task<bool> EditAsync(CategoryUpdateInputModel categoryUpdateInputModel);

        Task<bool> ArchiveAsync(string id);

        Task<bool> RecoverAsync(string id);

        bool Exists(string id);

        Task<TModel> GetAsync<TModel>(string id);

        Task<IEnumerable<SelectListItem>> AllToSelectListItemsAsync();

        Task<TModel> GetArchivedAsync<TModel>(string id);
    }
}
