namespace TwentyFirst.Common.Models.Interviews
{
    using Data.Models;
    using Extensions;
    using Mapping.Contracts;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class InterviewAdminListViewModel : IMapFrom<Interview>
    {
        public string Id { get; set; }

        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Display(Name = "Интервюиран")]
        public string Interviewed { get; set; }

        [Display(Name = "Публикувана")]
        public DateTime PublishedOn { get; set; }

        [Display(Name = "Добавил")]
        public string CreatorUserName { get; set; }

        [Display(Name = "Публикувана")]
        public string PublishedOnString
            => this.PublishedOn.UtcToEst().ToFormattedString();
    }
}
