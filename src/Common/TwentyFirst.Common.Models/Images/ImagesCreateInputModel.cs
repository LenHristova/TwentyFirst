namespace TwentyFirst.Common.Models.Images
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Attributes.ValidationAttributes;
    using AutoMapper;
    using Constants;
    using Data.Models;
    using Mapping.Contracts;
    using Microsoft.AspNetCore.Http;

    public class ImagesCreateInputModel : IMapTo<Image>, IHaveCustomMappings
    {
        [Required(ErrorMessage = ValidationErrorMessages.Required)]
        [MaxLength(200, ErrorMessage = ValidationErrorMessages.MaxLength)]
        [MinLength(5, ErrorMessage = ValidationErrorMessages.MinLength)]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Required(ErrorMessage = ValidationErrorMessages.Required)]
        [MinLength(5, ErrorMessage = ValidationErrorMessages.MinLength)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = ValidationErrorMessages.Required)]
        [MaxLength(300, ErrorMessage = ValidationErrorMessages.MaxLength)]
        [MinLength(2, ErrorMessage = ValidationErrorMessages.MinLength)]
        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Required(ErrorMessage = ValidationErrorMessages.RequiredImageUpload)]
        [MaxElementsCount(10, ErrorMessage = ValidationErrorMessages.MaxImageUpload)]
        [Display(Name = "Снимки")]
        public IEnumerable<IFormFile> Images { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ImagesCreateInputModel, Image>()
                .ForMember(dest => dest.Id, x => x.Ignore())
                .ForMember(dest => dest.Url, x => x.Ignore())
                .ForMember(dest => dest.ThumbUrl, x => x.Ignore())
                .ForMember(dest => dest.CreatedOn, x => x.Ignore())
                .ForMember(dest => dest.IsDeleted, x => x.Ignore())
                .ForMember(dest => dest.CreatorId, x => x.Ignore())
                .ForMember(dest => dest.Creator, x => x.Ignore())
                .ForMember(dest => dest.Editors, x => x.Ignore());
        }
    }
}
