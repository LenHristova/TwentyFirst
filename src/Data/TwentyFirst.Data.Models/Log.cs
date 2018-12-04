namespace TwentyFirst.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Log : BaseEntity<string>
    {
        [Required]
        public string Message { get; set; }

        public int EventId { get; set; }

        [Required]
        public string LogLevel { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
