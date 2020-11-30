using System;
namespace ProjectsNetwork.Services.IServices
{
    public interface IAuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
    }
}
