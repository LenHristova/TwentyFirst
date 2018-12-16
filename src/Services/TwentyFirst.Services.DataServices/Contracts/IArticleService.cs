namespace TwentyFirst.Services.DataServices.Contracts
{
    using Common.Models.Articles;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IArticleService
    {
        Task<string> CreateAsync(ArticleCreateInputModel articleCreateInputModel, string creatorId);

        bool Edit(ArticleEditInputModel articleUpdateInputModel, string editorId);

        Task<IEnumerable<SelectListItem>> AllToSelectListItemsAsync();

        Task<TModel> GetAsync<TModel>(string id);

        Task<IEnumerable<TModel>> GetLastAddedFromCategoriesAsync<TModel>(IEnumerable<string> ids, int count);

        Task<IEnumerable<TModel>> AllAsync<TModel>();

        bool Exists(string id);

        bool Delete(string articleId, string editorId);
    }
}
