namespace TwentyFirst.Common.Models.Polls
{
    using System;
    using Data.Models;
    using Extensions;
    using Mapping.Contracts;

    public class PollListViewModel : IMapFrom<Poll>
    {
        public string Id { get; set; }

        public string Question { get; set; }

        public string CreatorUserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedOnString
            => this.CreatedOn.UtcToEst().ToFormattedString();
    }
}
