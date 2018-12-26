namespace TwentyFirst.Common.Models.Articles
{
    using Data.Models;
    using Mapping.Contracts;

    public class ArticleBaseViewModel : IMapFrom<Article>
    {
        public string Id { get; set; }

        public string Title { get; set; }
    }
}
