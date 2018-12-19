namespace TwentyFirst.Web.Areas.Administration.Controllers
{
    using Common.Constants;
    using Common.Models.Articles;
    using Data.Models;
    using Filters;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Linq;
    using System.Threading.Tasks;

    [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
    public class ArticlesController : AdministrationController
    {
        private readonly IArticleService articleService;
        private readonly UserManager<User> userManager;

        public ArticlesController(IArticleService articleService, UserManager<User> userManager)
        {
            this.articleService = articleService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var articles = await this.articleService.AllAsync<ArticleListViewModel>();

            var onePageOfArticles = await articles.ToList()
                .PaginateAsync(pageNumber, GlobalConstants.AdministrationArticlesOnPageCount);

            return this.View(onePageOfArticles);
        }

        public IActionResult Create() => this.View();

        [HttpPost]
        public async Task<IActionResult> Create(ArticleCreateInputModel articleCreateInputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(articleCreateInputModel);
            }

            var userId = this.userManager.GetUserId(this.User);
            var article = await this.articleService.CreateAsync(articleCreateInputModel, userId);

            return RedirectToAction("Details", "Articles", new { article.Id });
        }

        public async Task<IActionResult> Edit(string id)
        {
            var article = await this.articleService.GetAsync<ArticleEditInputModel>(id);
            return this.View(article);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ArticleEditInputModel articleUpdateInputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(articleUpdateInputModel);
            }

            var userId = this.userManager.GetUserId(this.User);
            var article = await this.articleService.Edit(articleUpdateInputModel, userId);
            return RedirectToAction("Details", "Articles", new { article.Id });
        }

        public async Task<IActionResult> Delete(string id)
        {
            var article = await this.articleService.GetAsync<ArticleDeleteViewModel>(id);
            return this.View(article);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, string name)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.articleService.Delete(id, userId);

            return RedirectToAction(nameof(Index));
        }
    }
}
