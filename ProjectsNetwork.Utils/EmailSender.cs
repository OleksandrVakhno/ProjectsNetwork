using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using ProjectsNetwork.Services;
using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace ProjectsNetwork.Utils
{
    public class EmailSender : IEmailSender
    {
        public AuthMessageSenderOptions Options;
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("ProjectsNtwk@gmail.com", Options.SendGridUser),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}
