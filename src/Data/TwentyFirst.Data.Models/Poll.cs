namespace TwentyFirst.Data.Models
{
    using System;
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

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<PollOption> Options { get; set; } = new HashSet<PollOption>();

        public virtual ICollection<PollVote> Votes { get; set; } = new HashSet<PollVote>();
    }
}
