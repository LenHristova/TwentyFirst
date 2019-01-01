namespace TwentyFirst.Common.Models.Polls
{
    using Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Models;
    using Mapping.Contracts;

    public class PollDetailsViewModel : IMapFrom<Poll>
    {
        public string Question { get; set; }

        public string CreatorUserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsActive { get; set; }

        public ICollection<PollOptionViewModel> Options { get; set; }

        public string CreatedOnString
            => this.CreatedOn.UtcToEst().ToFormattedString();

        public int AllVotes => this.Options.Sum(o => o.Votes);

        public string IsActiveAsSymbol
            => this.IsActive ? "&#10004;" : "&#10008;";
    }
}
