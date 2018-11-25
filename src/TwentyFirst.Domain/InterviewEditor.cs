namespace TwentyFirst.Domain
{
    public class InterviewEditor
    {
        public string InterviewId { get; set; }
        public virtual Interview Interview { get; set; }

        public string EditorId { get; set; }
        public virtual User Editor { get; set; }
    }
}
