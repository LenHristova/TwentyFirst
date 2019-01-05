namespace TwentyFirst.Web.Components
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Constants;
    using Common.Models.Articles;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;

    [ViewComponent(Name = "important-articles-by-categories")]
    public class ImportantArticlesByCategoriesViewComponent : ViewComponent
    {
        private readonly IArticleService articleService;

        public ImportantArticlesByCategoriesViewComponent(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<string> ids, string excludeArticleId)
        {
            var connectedArticleViewModels = await this.articleService
                .LatestImportantFromCategoriesAsync<ArticleViewModel>(
                    ids, GlobalConstants.ArticlesCountForFromCategoriesSection);
            return this.View(connectedArticleViewModels.Where(a => a.Id != excludeArticleId));
        }
    }
}
