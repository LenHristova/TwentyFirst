namespace TwentyFirst.Common.Models.Interviews
{
    using System;
    using System.Collections.Generic;
    using Data.Models;
    using Extensions;
    using Mapping.Contracts;

    public class InterviewAllEditsViewModel : IMapFrom<Interview>
    {
        public string Title { get; set; }

        public string CreatorUserName { get; set; }

        public DateTime PublishedOn { get; set; }

        public IEnumerable<InterviewEditsViewModel> Edits { get; set; }

        public string PublishedOnString
            => this.PublishedOn.UtcToEstFormatted().ToFormattedString();
    }
}
