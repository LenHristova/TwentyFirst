namespace TwentyFirst.Common.Models.Polls
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Extensions;
    using Mapping.Contracts;

    public class PollListViewModel : IMapFrom<Poll>
    {
        public string Id { get; set; }

        [Display(Name = "Въпрос")]
        public string Question { get; set; }

        [Display(Name = "Добавена от")]
        public string CreatorUserName { get; set; }

        public DateTime CreatedOn { get; set; }

        [Display(Name = "Добавена на")]
        public string CreatedOnString
            => this.CreatedOn.UtcToEst().ToFormattedString();
    }
}
