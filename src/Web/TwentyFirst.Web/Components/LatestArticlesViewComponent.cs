namespace TwentyFirst.Web.Components
{
    using Common.Constants;
    using Common.Models.Articles;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Linq;
    using System.Threading.Tasks;

    [ViewComponent(Name = "latest-articles")]
    public class LatestArticlesViewComponent : ViewComponent
    {
        private readonly IArticleService articleService;

        public LatestArticlesViewComponent(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var connectedArticleViewModels = await this.articleService
                .LatestAsync<ArticleListViewModel>(
                    GlobalConstants.ArticlesCountForLatestSection);
            return this.View(connectedArticleViewModels.ToList());
        }
    }
}
