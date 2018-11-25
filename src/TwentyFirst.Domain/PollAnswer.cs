namespace TwentyFirst.Domain
{
    using System.ComponentModel.DataAnnotations;

    public class PollAnswer : BaseEntity<string>
    {
        [Required]
        public string PollId { get; set; }
        public virtual Poll Poll { get; set; }

        [Required]
        [MaxLength(500)]
        public string Answer { get; set; }

        public int Votes { get; set; }
    }
}
