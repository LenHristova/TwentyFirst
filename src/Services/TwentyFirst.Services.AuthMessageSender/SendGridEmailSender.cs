namespace TwentyFirst.Services.AuthMessageSender
{
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Options;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System.Threading.Tasks;

    public class SendGridEmailSender : IEmailSender
    {
        public SendGridEmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; }

        public Task SendEmailAsync(string recipient, string subject, string message)
        => Execute(
            this.Options.SendGridKey,
            this.Options.SendGridDefaultEmailSender,
            this.Options.SendGridUser,
            recipient,
            subject,
            message);


        public Task Execute(
            string apiKey, 
            string apiSender, 
            string apiSenderName, 
            string recipient, 
            string subject, 
            string message)
        {
            var client = new SendGridClient(apiKey);
            var sendGridMessage = new SendGridMessage
            {
                From = new EmailAddress(apiSender, apiSenderName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            sendGridMessage.AddTo(new EmailAddress(recipient));
            sendGridMessage.SetClickTracking(false, false);

            return client.SendEmailAsync(sendGridMessage);
        }
    }
}
