namespace TwentyFirst.Common.Models.Articles
{
    using System;
    using Data.Models;
    using Images;
    using Mapping.Contracts;

    public class ArticleListViewModel : IMapFrom<Article>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Lead { get; set; }

        public string Author { get; set; }

        public DateTime PublishedOn { get; set; }

        public ImageThumbBaseViewModel Image { get; set; }
    }
}
