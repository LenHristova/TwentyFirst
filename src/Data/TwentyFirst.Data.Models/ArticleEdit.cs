namespace TwentyFirst.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ArticleEdit : BaseEntity<string>
    {
        [Required]
        public string ArticleId { get; set; }
        public virtual Article Article { get; set; }

        [Required]
        public string EditorId { get; set; }
        public virtual User Editor { get; set; }

        public DateTime EditDateTime { get; set; }
    }
}
