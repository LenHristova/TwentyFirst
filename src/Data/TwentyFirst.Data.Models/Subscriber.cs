namespace TwentyFirst.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Subscriber : BaseEntity<string>
    {
        [Required]
        [MaxLength(300)]
        public string Email { get; set; }

        [Required]
        [MaxLength(300)]
        public string ConfirmationCode { get; set; }

        public bool IsConfirmed { get; set; }
    }
}
