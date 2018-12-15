namespace TwentyFirst.Web.Components
{
    using Common.Constants;
    using Common.Models.Articles;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ViewComponent(Name = "articles-by-categories")]
    public class ArticlesByCategoriesViewComponent : ViewComponent
    {
        private readonly IArticleService articleService;

        public ArticlesByCategoriesViewComponent(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<string> ids)
        {
            var connectedArticleViewModels = await this.articleService
                .GetLastAddedFromCategoriesAsync<ArticleBaseViewModel>(
                    ids,
                    GlobalConstants.ArticlesCountForFromCategoriesSection);
            return this.View(connectedArticleViewModels);
        }
    }
}
