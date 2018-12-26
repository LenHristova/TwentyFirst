namespace TwentyFirst.Common.Models.Articles
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;

    public class ConnectedArticlesChooseInputModel
    {
        public IEnumerable<string> ConnectedArticlesIds { get; set; }

        public IEnumerable<ArticleBaseViewModel> ArticleBaseViewModels { get; set; }

        public IEnumerable<SelectListItem> Articles => this.ArticleBaseViewModels
            .Select(a => new SelectListItem
            {
                Value = a.Id,
                Text = a.Title
            })
            .ToList();
    }
}
