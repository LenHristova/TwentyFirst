namespace TwentyFirst.Common.Models.Articles
{
    using Constants;
    using Data.Models;
    using Mapping.Contracts;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Attributes.ValidationAttributes;

    public class ArticleCreateInputModel : IMapTo<Article>
    {
        [Required(ErrorMessage = ValidationErrorMessages.Required)]
        [MaxLength(200, ErrorMessage = ValidationErrorMessages.MaxLength)]
        [MinLength(3, ErrorMessage = ValidationErrorMessages.MinLength)]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Required(ErrorMessage = ValidationErrorMessages.Required)]
        [MinLength(100, ErrorMessage = ValidationErrorMessages.MinLength)]
        [Display(Name = "Съдържание")]
        public string Content { get; set; }

        [MaxLength(200, ErrorMessage = ValidationErrorMessages.MaxLength)]
        [MinLength(2, ErrorMessage = ValidationErrorMessages.MinLength)]
        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Display(Name = "Снимка")]
        public string ImageId { get; set; }

        [Display(Name = "Топ новина?")]
        public bool IsTop { get; set; }

        [Display(Name = "Важна новина?")]
        public bool IsImportant { get; set; }

        [MinElementsCount(1, ErrorMessage = ValidationErrorMessages.RequiredCategories)]
        [Display(Name = "Категории")]
        public IEnumerable<string> CategoriesIds { get; set; }

        [Display(Name = "Свързани новини")]
        [MaxElementsCount(5, ErrorMessage = ValidationErrorMessages.MaxConnectedArticles)]
        public IEnumerable<string> ConnectedArticlesIds { get; set; }
    }
}
