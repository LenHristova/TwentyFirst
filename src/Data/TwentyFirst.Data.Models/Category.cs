namespace TwentyFirst.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Category : BaseEntity<string>
    {
        [Required]
        [MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        
        public int Order { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<ArticleCategory> Articles { get; set; } = new HashSet<ArticleCategory>();
    }
}
