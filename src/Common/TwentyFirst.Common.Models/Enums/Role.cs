namespace TwentyFirst.Common.Models.Enums
{
    using System.ComponentModel.DataAnnotations;
    using Constants;

    public enum Role
    {
        [Display(Name = GlobalConstants.MasterAdministratorRoleName)]
        MasterAdmin = 1,
        [Display(Name = GlobalConstants.AdministratorRoleName)]
        Admin = 2,
        [Display(Name = GlobalConstants.EditorRoleName)]
        Editor = 3
    }
}
