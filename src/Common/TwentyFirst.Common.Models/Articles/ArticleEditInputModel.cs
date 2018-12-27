namespace TwentyFirst.Common.Models.Articles
{
    using Attributes.ValidationAttributes;
    using Constants;
    using Data.Models;
    using Mapping.Contracts;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AutoMapper;
    using Images;

    public class ArticleEditInputModel : IMapTo<Article>, IHaveCustomMappings
    {
        public string Id { get; set; }

        [Required(ErrorMessage = ValidationErrorMessages.Required)]
        [MaxLength(200, ErrorMessage = ValidationErrorMessages.MaxLength)]
        [MinLength(3, ErrorMessage = ValidationErrorMessages.MinLength)]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Required(ErrorMessage = ValidationErrorMessages.Required)]
        [MaxLength(200, ErrorMessage = ValidationErrorMessages.MaxLength)]
        [MinLength(3, ErrorMessage = ValidationErrorMessages.MinLength)]
        [Display(Name = "Лийд")]
        public string Lead { get; set; }

        [Required(ErrorMessage = ValidationErrorMessages.Required)]
        [MinLength(100, ErrorMessage = ValidationErrorMessages.MinLength)]
        [Display(Name = "Съдържание")]
        public string Content { get; set; }

        [MaxLength(200, ErrorMessage = ValidationErrorMessages.MaxLength)]
        [MinLength(2, ErrorMessage = ValidationErrorMessages.MinLength)]
        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Display(Name = "Снимка")]
        public ImageBaseInputModel Image { get; set; }

        [Display(Name = "Топ новина?")]
        public bool IsTop { get; set; }

        [Display(Name = "Важна новина?")]
        public bool IsImportant { get; set; }

        [Required(ErrorMessage = ValidationErrorMessages.RequiredCategories)]
        [Display(Name = "Категории")]
        public IEnumerable<string> CategoriesIds { get; set; }

        [Display(Name = "Свързани новини")]
        [MaxElementsCount(5, ErrorMessage = ValidationErrorMessages.MaxConnectedArticles)]
        public IEnumerable<string> ConnectedArticlesIds { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Article, ArticleEditInputModel>()
                .ForMember(dest => dest.CategoriesIds,
                    x => x.MapFrom(src => src.Categories.Where(ca => !ca.Category.IsDeleted).Select(ac => ac.CategoryId)))
                .ForMember(dest => dest.ConnectedArticlesIds,
                    x => x.MapFrom(src => src.ConnectedTo.Where(ca => !ca.ConnectedTo.IsDeleted).Select(ac => ac.ConnectedToId)));
        }
    }
}
