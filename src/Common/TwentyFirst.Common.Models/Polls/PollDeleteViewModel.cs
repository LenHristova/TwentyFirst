namespace TwentyFirst.Common.Models.Polls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Data.Models;
    using Extensions;
    using Mapping.Contracts;

    public class PollDeleteViewModel : IMapFrom<Poll>
    {
        public string Id { get; set; }

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
