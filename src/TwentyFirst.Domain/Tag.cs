namespace TwentyFirst.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Tag : BaseEntity<string>
    {
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<ArticleTag> Articles { get; set; } = new HashSet<ArticleTag>();

        public virtual ICollection<InterviewTag> Interviews { get; set; } = new HashSet<InterviewTag>();
    }
}
