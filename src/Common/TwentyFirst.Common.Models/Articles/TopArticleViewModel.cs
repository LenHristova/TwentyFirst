namespace TwentyFirst.Common.Models.Articles
{
    using Constants;
    using System;

    public class TopArticleViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

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
    }
}
