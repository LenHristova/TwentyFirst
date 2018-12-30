namespace TwentyFirst.Web.Components
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Constants;
    using Common.Models.Articles;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;

    [ViewComponent(Name = "important-articles")]
    public class ImportantArticlesViewComponent : ViewComponent
    {
        private readonly IArticleService articleService;

        public ImportantArticlesViewComponent(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var connectedArticleViewModels = await this.articleService
                .LatestImportantAsync<ArticleViewModel>(GlobalConstants.ImportantArticlesCount);
            return this.View(connectedArticleViewModels);
        }
    }
}
