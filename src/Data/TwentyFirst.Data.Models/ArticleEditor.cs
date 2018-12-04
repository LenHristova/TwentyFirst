namespace TwentyFirst.Data.Models
{
    public class ArticleEditor
    {
        public string ArticleId { get; set; }
        public virtual Article Article { get; set; }

        public string EditorId { get; set; }
        public virtual User Editor { get; set; }
    }
}
