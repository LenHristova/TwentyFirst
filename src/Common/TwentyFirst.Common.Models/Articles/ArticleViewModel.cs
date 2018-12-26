namespace TwentyFirst.Common.Models.Articles
{
    using Constants;
    using Data.Models;
    using Images;
    using Mapping.Contracts;
    using System;

    public class ArticleViewModel : IMapFrom<Article>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public DateTime PublishedOn { get; set; }

        public string ShortTitle
        {
            get
            {
                var description = this.Title ?? string.Empty;
                var symbolsToGet = Math.Min(
                    description.Length, GlobalConstants.ConnectedArticleShortTitleMaxLength);
                return this.Title?.Substring(0, symbolsToGet) + "...";
            }
        }

        public ImageThumbBaseViewModel Image { get; set; }
    }
}
