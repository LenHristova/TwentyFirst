namespace TwentyFirst.Services.DataServices.Contracts
{
    using Common.Constants;
    using Common.Exceptions;
    using Common.Models.Articles;
    using Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IArticleService
    {
        Task<IEnumerable<TModel>> LatestAsync<TModel>(int count);

        Task<IEnumerable<TModel>> LatestTopAsync<TModel>(int count);

        Task<IEnumerable<TModel>> LatestFromCategoryAsync<TModel>(string categoryId, int count);

        Task<IEnumerable<TModel>> LatestImportantAsync<TModel>(int count);

        Task<IEnumerable<TModel>> LatestImportantFromCategoriesAsync<TModel>(
            IEnumerable<string> ids, int count);

        /// <summary>
        /// Returns collection of important articles' ids for the day
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TModel>> AllImportantForTheDayAsync<TModel>();

        Task<Article> CreateAsync(ArticleCreateInputModel articleCreateInputModel, string creatorId);

        Task<Article> EditAsync(ArticleEditInputModel articleEditInputModel, string editorId);

        Task DeleteAsync(string articleId, string editorId);

        /// <summary>
        /// Gets article by id and project it to given model.
        /// Throw InvalidArticleException if id is not present.
        /// </summary>
        /// <exception cref="InvalidArticleException"></exception>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TModel> GetAsync<TModel>(string id);

        /// <summary>
        /// Gets article by id.
        /// Throw InvalidArticleException if id is not present.
        /// </summary>
        /// <exception cref="InvalidArticleException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Article> GetAsync(string id);

        void ThrowIfAnyNotExist(IEnumerable<string> ids);
    }
}
