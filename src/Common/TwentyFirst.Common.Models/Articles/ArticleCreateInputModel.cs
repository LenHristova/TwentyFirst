namespace TwentyFirst.Common.Models.Articles
{
    using Attributes.ValidationAttributes;
    using AutoMapper;
    using Constants;
    using Data.Models;
    using Images;
    using Mapping.Contracts;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class ArticleCreateInputModel : IMapTo<Article>, IHaveCustomMappings
    {
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
        public ImageForArticleInputModel Image { get; set; }
        //[Display(Name = "Снимка")]
        //public string ImageId { get; set; }

        //public string ImageThumbUrl { get; set; }

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
            configuration.CreateMap<ArticleCreateInputModel, Article>()
                .ForMember(dest => dest.ImageId, x => x.MapFrom(src => src.Image.Id))
                .ForMember(dest => dest.PublishedOn, x => x.Ignore())
                .ForMember(dest => dest.Image, x => x.Ignore())
                .ForMember(dest => dest.IsDeleted, x => x.Ignore())
                .ForMember(dest => dest.CreatorId, x => x.Ignore())
                .ForMember(dest => dest.Creator, x => x.Ignore()); 
        }
    }
}
