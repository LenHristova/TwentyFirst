namespace TwentyFirst.Common.Models.Interviews
{
    using AutoMapper;
    using Constants;
    using Data.Models;
    using Images;
    using Mapping.Contracts;
    using System.ComponentModel.DataAnnotations;

    public class InterviewCreateInputModel : IMapTo<Interview>, IHaveCustomMappings
    {
        [Required(ErrorMessage = ValidationErrorMessages.Required)]
        [MaxLength(300, ErrorMessage = ValidationErrorMessages.MaxLength)]
        [MinLength(3, ErrorMessage = ValidationErrorMessages.MinLength)]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Required(ErrorMessage = ValidationErrorMessages.Required)]
        [Display(Name = "Съдържание")]
        public string Content { get; set; }

        [Required(ErrorMessage = ValidationErrorMessages.Required)]
        [MaxLength(200, ErrorMessage = ValidationErrorMessages.MaxLength)]
        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Required(ErrorMessage = ValidationErrorMessages.Required)]
        [MaxLength(200, ErrorMessage = ValidationErrorMessages.MaxLength)]
        [MinLength(3, ErrorMessage = ValidationErrorMessages.MinLength)]
        [Display(Name = "Интервюиран")]
        public string Interviewed { get; set; }

        [Required(ErrorMessage = ValidationErrorMessages.Required)]
        [Display(Name = "Снимка")]
        public ImageBaseInputModel Image { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<InterviewCreateInputModel, Interview>()
                .ForMember(dest => dest.ImageId, x => x.MapFrom(src => src.Image.Id))
                .ForMember(dest => dest.Image, x => x.Ignore())
                .ForMember(dest => dest.PublishedOn, x => x.Ignore())
                .ForMember(dest => dest.IsDeleted, x => x.Ignore())
                .ForMember(dest => dest.CreatorId, x => x.Ignore())
                .ForMember(dest => dest.Creator, x => x.Ignore());
        }
    }
}
