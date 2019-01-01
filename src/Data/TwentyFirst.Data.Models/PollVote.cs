namespace TwentyFirst.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PollVote : BaseEntity<string>
    {
        [Required]
        public string Ip { get; set; }

        [Required]
        public string PollId { get; set; }
        public virtual Poll Poll { get; set; }
    }
}
