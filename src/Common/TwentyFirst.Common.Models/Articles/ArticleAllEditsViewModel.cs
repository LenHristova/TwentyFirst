namespace TwentyFirst.Common.Models.Articles
{
    using System;
    using System.Collections.Generic;
    using Data.Models;
    using Extensions;
    using Mapping.Contracts;

    public class ArticleAllEditsViewModel : IMapFrom<Article>
    {
        public string Title { get; set; }

        public string CreatorUserName { get; set; }

        public DateTime PublishedOn { get; set; }

        public IEnumerable<ArticleEditsViewModel> Edits { get; set; }

        public string PublishedOnString
            => this.PublishedOn.UtcToEstFormatted().ToFormattedString();
    }
}
