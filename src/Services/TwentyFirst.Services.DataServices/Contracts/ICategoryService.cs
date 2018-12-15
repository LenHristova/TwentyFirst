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

        TModel Get<TModel>(string id);

        Task<IEnumerable<SelectListItem>> AllToSelectListItemsAsync();

        TModel GetArchived<TModel>(string id);
    }
}
