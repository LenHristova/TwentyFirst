namespace TwentyFirst.Web.Controllers
{
    using Common.Constants;
    using Common.Models.Articles;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Linq;
    using System.Threading.Tasks;

    public class ArticlesController : BaseController
    {
        private readonly IArticleService articleService;

        public ArticlesController(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        public async Task<IActionResult> Index(int? pageNumber, string categoryId = null)
        {
            var articles = categoryId == null
                    ? await this.articleService
                        .LatestAsync<ArticleListViewModel>(GlobalConstants.MaxArticlesCountToGet)
                    : await this.articleService
                        .LatestFromCategoryAsync<ArticleListViewModel>(categoryId, GlobalConstants.MaxArticlesCountToGet);

            var onePageOfArticles = await articles.ToList()
                .PaginateAsync(pageNumber, GlobalConstants.ArticlesOnPageCount);

            return this.View(onePageOfArticles);
        }

        public async Task<IActionResult> Details(string id)
        {
            var article = await this.articleService.GetAsync<ArticleDetailsViewModel>(id);
            return this.View(article);
        }
    }
}
