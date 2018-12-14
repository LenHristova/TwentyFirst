namespace TwentyFirst.Common.Models.Articles
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;

    public class ConnectedArticlesChooseInputModel
    {
        public IEnumerable<string> ConnectedArticlesIds { get; set; }

        public IEnumerable<SelectListItem> Articles { get; set; }
    }
}
