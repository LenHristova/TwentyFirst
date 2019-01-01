namespace TwentyFirst.Common.Models.Polls
{
    using Attributes.ValidationAttributes;
    using Constants;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class PollCreateInputModel : IValidatableObject
    {
        [Required(ErrorMessage = ValidationErrorMessages.Required)]
        [MaxLength(200, ErrorMessage = ValidationErrorMessages.MaxLength)]
        [MinLength(5, ErrorMessage = ValidationErrorMessages.MinLength)]
        [Display(Name = "Въпрос")]
        public string Question { get; set; }

        [Required(ErrorMessage = ValidationErrorMessages.Required)]
        [MinElementsCount(2, ErrorMessage = ValidationErrorMessages.MinPollOptions)]
        public virtual ICollection<string> Options { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Options.Any(o => string.IsNullOrWhiteSpace(o) || o.Length < 2))
            {
                yield return new ValidationResult(string.Format(ValidationErrorMessages.MinLength, null, 2));
            }
        }
    }
}
