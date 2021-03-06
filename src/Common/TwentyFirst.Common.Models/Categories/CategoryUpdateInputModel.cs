﻿namespace TwentyFirst.Common.Models.Categories
{
    using System.ComponentModel.DataAnnotations;
    using Constants;
    using Data.Models;
    using Mapping.Contracts;

    public class CategoryUpdateInputModel : IMapTo<Category>, IMapFrom<Category>
    {
        public string Id { get; set; }

        [Required(ErrorMessage = ValidationErrorMessages.Required)]
        [MaxLength(30, ErrorMessage = ValidationErrorMessages.MaxLength)]
        [MinLength(3, ErrorMessage = ValidationErrorMessages.MinLength)]
        [Display(Name = "Наименование")]
        public string Name { get; set; }
    }
}
