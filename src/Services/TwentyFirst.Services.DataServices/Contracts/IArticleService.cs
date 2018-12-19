namespace TwentyFirst.Services.DataServices.Contracts
{
    using Common.Models.Articles;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Data.Models;

    public interface IArticleService
    {
        Task<Article> CreateAsync(ArticleCreateInputModel articleCreateInputModel, string creatorId);

        Task<Article> Edit(ArticleEditInputModel articleEditInputModel, string editorId);

        Task<IEnumerable<SelectListItem>> AllToSelectListItemsAsync();

        /// <summary>
        /// Gets article by id and project it to given model.
        /// Throw InvalidArticleIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidArticleIdException"></exception>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TModel> GetAsync<TModel>(string id);

        /// <summary>
        /// Gets article by id.
        /// Throw InvalidArticleIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidArticleIdException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Article> GetAsync(string id);

        Task<IEnumerable<TModel>> GetLastAddedFromCategoriesAsync<TModel>(IEnumerable<string> ids, int count);

        Task<IEnumerable<TModel>> AllAsync<TModel>();

        Task Delete(string articleId, string editorId);
    }
}
