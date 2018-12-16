namespace TwentyFirst.Web.Areas.Administration.Controllers
{
    using Common.Models.Articles;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System;
    using System.Threading.Tasks;

    public class ArticlesController : AdministrationController
    {
        private readonly IArticleService articleService;
        private readonly UserManager<User> userManager;

        public ArticlesController(IArticleService articleService, UserManager<User> userManager)
        {
            this.articleService = articleService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await this.articleService.AllAsync<AdministrationArticleListViewModel>();

            return this.View(articles);
        }

        public IActionResult Create()
        {
            //TODO need article?
            return this.View(new ArticleCreateInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticleCreateInputModel articleCreateInputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(articleCreateInputModel);
            }

            var userId = this.userManager.GetUserId(this.User);
            var id = await this.articleService.CreateAsync(articleCreateInputModel, userId);
            if (id == null)
            {
                throw new Exception();
                //TODO thr custom exception
            }

            return RedirectToAction("Details", "Articles", new { id });
        }

        public async Task<IActionResult> Edit(string id)
        {
            var article = await this.articleService.GetAsync<ArticleEditInputModel>(id);
            if (id == null)
            {
                throw new Exception();
                //TODO thr custom exception
            }

            return this.View(article);
        }

        [HttpPost]
        public IActionResult Edit(ArticleEditInputModel articleUpdateInputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(articleUpdateInputModel);
            }

            var articleExists = this.articleService.Exists(articleUpdateInputModel.Id);
            if (!articleExists)
            {
                throw new Exception();
                //TODO thr custom exception
            }

            var userId = this.userManager.GetUserId(this.User);
            var success = this.articleService.Edit(articleUpdateInputModel, userId);
            if (!success)
            {
                throw new Exception();
                //TODO thr custom exception
            }

            return RedirectToAction("Details", "Articles", new { articleUpdateInputModel.Id });
        }

        public async Task<IActionResult> Delete(string id)
        {
            var article = await this.articleService.GetAsync<ArticleDeleteViewModel>(id);
            if (id == null)
            {
                throw new Exception();
                //TODO thr custom exception
            }

            return this.View(article);
        }

        [HttpPost]
        public IActionResult Delete(string id, string name)
        {
            var articleExists = this.articleService.Exists(id);
            if (!articleExists)
            {
                throw new Exception();
                //TODO thr custom exception
            }

            var userId = this.userManager.GetUserId(this.User);
            var success = this.articleService.Delete(id, userId);
            if (!success)
            {
                throw new Exception();
                //TODO thr custom exception
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
