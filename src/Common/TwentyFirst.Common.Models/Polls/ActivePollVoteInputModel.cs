namespace TwentyFirst.Common.Models.Polls
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Constants;
    using Data.Models;
    using Mapping.Contracts;

    public class ActivePollVoteInputModel : IMapFrom<Poll>
    {
        public string Id { get; set; }

        public string Question { get; set; }

        public string VoteIp { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = ValidationErrorMessages.RequiredSelect)]
        public int SelectedOptionId { get; set; }

        public IList<PollOptionChooseModel> Options { get; set; }
    }
}
