namespace TwentyFirst.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Subscriber : BaseEntity<string>
    {
        [Required]
        [MaxLength(300)]
        public string Email { get; set; }
    }
}
