namespace TwentyFirst.Services.DataServices.Contracts
{
    using Common.Exceptions;
    using Common.Models.Articles;
    using Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IArticleService
    {
        Task<Article> CreateAsync(ArticleCreateInputModel articleCreateInputModel, string creatorId);

        Task<Article> EditAsync(ArticleEditInputModel articleEditInputModel, string editorId);

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

        Task<IEnumerable<TModel>> ImportantFromCategoriesAsync<TModel>(IEnumerable<string> ids, int count);

        Task<IEnumerable<TModel>> LatestAsync<TModel>(int count);

        Task<IEnumerable<TModel>> AllAsync<TModel>();

        Task DeleteAsync(string articleId, string editorId);

        /// <summary>
        /// Returns collection of important articles' ids for the day
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TModel>> AllImportantForTheDay<TModel>();

        Task<IEnumerable<TModel>> LatestTopAsync<TModel>(int count);

        Task<IEnumerable<TModel>> ByCategoryAsync<TModel>(string id);
    }
}
