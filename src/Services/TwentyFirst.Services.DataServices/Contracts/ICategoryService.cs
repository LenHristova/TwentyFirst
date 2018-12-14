namespace TwentyFirst.Services.DataServices.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Models.Categories;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICategoryService
    { 
        Task<IEnumerable<TModel>> All<TModel>();

        Task<bool> CreateAsync(CategoryCreateInputModel categoryCreateInputModel);

        Task<bool> DeleteAsync(string id);

        Task<bool> EditAsync(CategoryUpdateInputModel categoryUpdateInputModel);

        bool Exists(string id);

        TModel Get<TModel>(string id);

        Task<IEnumerable<SelectListItem>> AllToSelectListItemsAsync();
    }
}
