﻿namespace TwentyFirst.Web.Areas.Administration.Controllers
{
    using Common.Constants;
    using Common.Models.Articles;
    using Data.Models;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Linq;
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

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var articles = await this.articleService
                    .LatestAsync<ArticleAdminListViewModel>(GlobalConstants.AdminMaxArticlesCountToGet);

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
            var article = await this.articleService.EditAsync(articleUpdateInputModel, userId);
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
            await this.articleService.DeleteAsync(id, userId);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AllEdits(string id, int? pageNumber)
        {
            var articleEdits = await this.articleService.GetAsync<ArticleAllEditsViewModel>(id);

            if (articleEdits.Edits != null && articleEdits.Edits.Any())
            {
                articleEdits.Edits = await articleEdits.Edits
                    .OrderByDescending(ae => ae.EditDateTime)
                    .ToList()
                    .PaginateAsync(pageNumber, GlobalConstants.AdministrationArticleEditsOnPageCount);
            }

            return this.View(articleEdits);
        }
    }
}
