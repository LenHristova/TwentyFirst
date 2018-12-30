namespace TwentyFirst.Common.Models.Interviews
{
    using Data.Models;
    using Extensions;
    using Mapping.Contracts;
    using System;

    public class InterviewEditsViewModel : IMapFrom<InterviewEdit>
    {
        public string EditorUserName { get; set; }

        public DateTime EditDateTime { get; set; }

        public string EditDateTimeString
            => this.EditDateTime.UtcToEstFormatted().ToFormattedString();
    }
}
