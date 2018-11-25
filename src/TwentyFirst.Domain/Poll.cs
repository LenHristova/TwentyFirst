namespace TwentyFirst.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Poll : BaseEntity<string>
    {
        [Required]
        [MaxLength(500)]
        public string Question { get; set; }

        [Required]
        public string CreatorId { get; set; }
        public virtual User Creator { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<PollAnswer> Answers { get; set; } = new HashSet<PollAnswer>();
    }
}
