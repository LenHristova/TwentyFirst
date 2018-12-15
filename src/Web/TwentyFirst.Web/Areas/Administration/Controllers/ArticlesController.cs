namespace TwentyFirst.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Common.Models.Articles;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;

    public class ArticlesController : AdministrationController
    {
        private readonly IArticleService articleService;
        private readonly UserManager<User> userManager;

        public ArticlesController(IArticleService articleService, UserManager<User> userManager)
        {
            this.articleService = articleService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Create()
        {
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
    }
}
