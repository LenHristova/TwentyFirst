namespace TwentyFirst.Common.Models.Interviews
{
    using System;
    using Data.Models;
    using Mapping.Contracts;

    public class InterviewEditsViewModel : IMapFrom<InterviewEdit>
    {
        public string EditorUserName { get; set; }

        public DateTime EditDateTime { get; set; }
    }
}
