namespace TwentyFirst.Data.Models
{
    public class ArticleToArticle
    {
        public string ConnectedToId { get; set; }
        public virtual Article ConnectedTo { get; set; }

        public string ConnectedFromId { get; set; }
        public virtual Article ConnectedFrom { get; set; }
    }
}
