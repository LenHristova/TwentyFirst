namespace TwentyFirst.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Interview : BaseEntity<string>
    {
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [MaxLength(200)]
        public string Author { get; set; }

        [Required]
        public string Interviewed { get; set; }

        public DateTime PublishedOn { get; set; }

        [Required]
        public string ImageId { get; set; }
        public virtual Image Image { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public string CreatorId { get; set; }
        public virtual User Creator { get; set; }

        public virtual ICollection<InterviewEditor> Editors { get; set; } = new HashSet<InterviewEditor>();

        public virtual ICollection<InterviewTag> Tags { get; set; } = new HashSet<InterviewTag>();
    }
}
