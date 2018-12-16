namespace TwentyFirst.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Article : BaseEntity<string>
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(200)]
        public string Lead { get; set; }

        [Required]
        public string Content { get; set; }

        [MaxLength(200)]
        public string Author { get; set; }

        public DateTime PublishedOn { get; set; }

        public string ImageId { get; set; }
        public virtual Image Image { get; set; }

        public bool IsTop { get; set; }

        public bool IsImportant { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public string CreatorId { get; set; }
        public virtual User Creator { get; set; }

        public virtual ICollection<ArticleEdit> Edits { get; set; } = new HashSet<ArticleEdit>();

        public virtual ICollection<ArticleCategory> Categories { get; set; } = new HashSet<ArticleCategory>();

        public virtual ICollection<ArticleToArticle> ConnectedTo { get; set; } = new HashSet<ArticleToArticle>();
    }
}
