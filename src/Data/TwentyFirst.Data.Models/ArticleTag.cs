namespace TwentyFirst.Data.Models
{
    public class ArticleTag
    {
        public string ArticleId { get; set; }
        public virtual Article Article { get; set; }

        public string TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
