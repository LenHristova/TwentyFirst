namespace TwentyFirst.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Image : BaseEntity<string>
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string ThumbUrl { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [MaxLength(300)]
        public string Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public string CreatorId { get; set; }
        public virtual User Creator { get; set; }

        public virtual ICollection<ImageEditor> Editors { get; set; } = new HashSet<ImageEditor>();
    }
}
