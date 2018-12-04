namespace TwentyFirst.Data.Models
{
    public class ArticleCategory
    {
        public string ArticleId { get; set; }
        public virtual Article Article { get; set; }
        
        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
