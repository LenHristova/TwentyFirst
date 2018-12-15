namespace TwentyFirst.Common.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum AlertMessageLevel
    {
        [Display(Name = "alert-success")]
        Success = 1,
        [Display(Name = "alert-danger")]
        Error = 2
    }
}
