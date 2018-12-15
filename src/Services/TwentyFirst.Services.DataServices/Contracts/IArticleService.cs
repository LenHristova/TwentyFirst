namespace TwentyFirst.Services.DataServices.Contracts
{
    using Common.Models.Articles;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IArticleService
    {
        Task<string> CreateAsync(ArticleCreateInputModel articleCreateInputModel, string creatorId);

        Task<IEnumerable<SelectListItem>> AllToSelectListItemsAsync();

        Task<TModel> GetAsync<TModel>(string id);

        Task<IEnumerable<TModel>> GetLastAddedFromCategoriesAsync<TModel>(IEnumerable<string> ids, int count);
    }
}
