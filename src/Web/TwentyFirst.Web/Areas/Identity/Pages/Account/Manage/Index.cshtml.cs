namespace TwentyFirst.Web.Areas.Identity.Pages.Account.Manage
{
    using Common.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using TwentyFirst.Data.Models;

    [Authorize(Roles = GlobalConstants.MasterAdministratorRoleName)]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IEmailSender emailSender;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = ValidationErrorMessages.Required)]
            [EmailAddress(ErrorMessage = ValidationErrorMessages.InvalidEmailFormat)]
            [Display(Name = "Фирмен имейл")]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new InvalidAccountException();
            }

            var userName = await userManager.GetUserNameAsync(user);
            var email = await userManager.GetEmailAsync(user);

            Username = userName;

            Input = new InputModel { Email = email };

            IsEmailConfirmed = await userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new InvalidAccountException();
            }

            var email = await userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new InvalidOperationException("Възникна грешка при смяната на емайла.");
                }
            }

            await signInManager.RefreshSignInAsync(user);
            StatusMessage = "Профилът беше променен успешно.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new InvalidAccountException();
            }

            var userId = await userManager.GetUserIdAsync(user);
            var email = await userManager.GetEmailAsync(user);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await emailSender.SendEmailAsync(
                email,
                "Потвъждение на имейл",
                $"Моля потвърдете акаунта си <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>кликайки тук</a>.");
            
            StatusMessage = GlobalConstants.ConfirmationEmailSend;
            return RedirectToPage();
        }
    }
}
