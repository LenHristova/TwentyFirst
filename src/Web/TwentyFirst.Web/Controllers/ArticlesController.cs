namespace TwentyFirst.Web.Controllers
{
    using System.Threading.Tasks;
    using Common.Models.Articles;
    using Filters;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;

    [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
    public class ArticlesController : BaseController
    {
        private readonly IArticleService articleService;

        public ArticlesController(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Details(string id)
        {
            var article = await this.articleService.GetAsync<ArticleDetailsViewModel>(id);
            return this.View(article);
        }
    }
}
