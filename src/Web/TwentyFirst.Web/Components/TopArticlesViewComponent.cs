namespace TwentyFirst.Web.Components
{
    using Common.Models.Articles;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Constants;

    [ViewComponent(Name = "top-articles")]
    public class TopArticlesViewComponent : ViewComponent
    {
        private readonly IArticleService articleService;

        public TopArticlesViewComponent(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var topArticles = await this.articleService
                .LatestTopAsync<TopArticleViewModel>(GlobalConstants.TopArticlesCount);

            return this.View(topArticles.ToList());
        }
    }
}
