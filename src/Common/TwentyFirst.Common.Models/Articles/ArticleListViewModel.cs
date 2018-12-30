namespace TwentyFirst.Common.Models.Articles
{
    using System;
    using Data.Models;
    using Extensions;
    using Images;
    using Mapping.Contracts;

    public class ArticleListViewModel : IMapFrom<Article>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Lead { get; set; }

        public DateTime PublishedOn { get; set; }

        public ImageThumbBaseViewModel Image { get; set; }

        public string PublishedOnString
            => this.PublishedOn.UtcToEstFormatted().ToFormattedString();
    }
}
