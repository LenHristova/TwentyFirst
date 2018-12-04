namespace TwentyFirst.Common.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum Role
    {
        [Display(Name = "Master Administrator")]
        MasterAdmin = 1,
        [Display(Name = "Administrator")]
        Admin = 2,
        Editor = 3
    }
}
