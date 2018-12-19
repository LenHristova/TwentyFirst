namespace TwentyFirst.Common.Models.Interviews
{
    using Constants;
    using System.ComponentModel.DataAnnotations;

    public class InterviewDeleteViewModel
    {
        public string Id { get; set; }

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

        [Display(Name = "Снимка")]
        public string ImageThumbUrl { get; set; }
    }
}
