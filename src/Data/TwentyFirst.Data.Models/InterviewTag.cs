namespace TwentyFirst.Data.Models
{
    public class InterviewTag
    {
        public string InterviewId { get; set; }
        public virtual Interview Interview { get; set; }

        public string TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
