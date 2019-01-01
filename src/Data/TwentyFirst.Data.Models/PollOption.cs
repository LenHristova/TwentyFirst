namespace TwentyFirst.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PollOption : BaseEntity<int>
    {
        [Required]
        public string PollId { get; set; }
        public virtual Poll Poll { get; set; }

        [Required]
        [MaxLength(300)]
        public string Value { get; set; }

        public int Votes { get; set; }
    }
}
