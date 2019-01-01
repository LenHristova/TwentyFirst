namespace TwentyFirst.Common.Models.Articles
{
    using Constants;
    using Data.Models;
    using Images;
    using Mapping.Contracts;
    using System;
    using Extensions;

    public class ArticleViewModel : IMapFrom<Article>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime PublishedOn { get; set; }

        public string ShortTitle
        {
            get
            {
                var description = this.Title ?? string.Empty;
                var symbolsToGet = Math.Min(
                    description.Length, GlobalConstants.ArticleShortTitleMaxLength);
                return this.Title?.Substring(0, symbolsToGet) + "...";
            }
        }

        public ImageThumbBaseViewModel Image { get; set; }

        public string PublishedOnString
            => this.PublishedOn.UtcToEst().ToFormattedString();
    }
}
