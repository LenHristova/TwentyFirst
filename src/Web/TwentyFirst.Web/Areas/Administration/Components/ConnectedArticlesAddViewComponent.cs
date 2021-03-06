﻿namespace TwentyFirst.Web.Areas.Administration.Components
{
    using Common.Models.Articles;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Constants;

    [ViewComponent(Name = "connected-articles")]
    public class ConnectedArticlesViewComponent : ViewComponent
    {
        private readonly IArticleService articleService;

        public ConnectedArticlesViewComponent(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<string> ids)
        => View(new ConnectedArticlesChooseInputModel
        {
            ConnectedArticlesIds = ids,
            ArticleBaseViewModels = await this.articleService
                .LatestAsync<ArticleBaseViewModel>(GlobalConstants.MaxArticlesCountToGet)
        });

    }
}
