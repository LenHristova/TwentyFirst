namespace TwentyFirst.Common.Models.Articles
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Mapping.Contracts;

    public class ArticleAdminListViewModel: IMapFrom<Article>
    {
        public string Id { get; set; }

        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Display(Name = "Публикувана")]
        public DateTime PublishedOn { get; set; }

        [Display(Name = "Топ")]
        public bool IsTop { get; set; }

        [Display(Name = "Важна")]
        public bool IsImportant { get; set; }

        [Display(Name = "Добавил")]
        public string CreatorUserName { get; set; }
    }
}
