using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Threading.Tasks;

namespace ProjectsNetwork.Utils
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            
            return Task.Run(delegate () { Console.WriteLine(htmlMessage); });

        }
    }
}
