namespace TwentyFirst.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class InterviewEdit : BaseEntity<string>
    {
        [Required]
        public string InterviewId { get; set; }
        public virtual Interview Interview { get; set; }

        [Required]
        public string EditorId { get; set; }
        public virtual User Editor { get; set; }

        public DateTime EditDateTime { get; set; }
    }
}
