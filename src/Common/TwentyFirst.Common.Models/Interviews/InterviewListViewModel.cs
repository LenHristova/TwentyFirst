namespace TwentyFirst.Common.Models.Interviews
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class InterviewListViewModel
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
    }
}
