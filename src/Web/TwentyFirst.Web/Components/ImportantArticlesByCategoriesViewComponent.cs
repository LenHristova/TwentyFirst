namespace TwentyFirst.Web.Components
{
    using System.Collections.Generic;
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

        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<string> ids)
        {
            var connectedArticleViewModels = await this.articleService
                .ImportantFromCategoriesAsync<ArticleBaseViewModel>(
                    ids,
                    GlobalConstants.ArticlesCountForFromCategoriesSection);
            return this.View(connectedArticleViewModels);
        }
    }
}
