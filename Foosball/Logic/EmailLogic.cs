using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KamilSzymborski.MailSenders;
using Microsoft.Extensions.Options;
using Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Foosball.Logic
{
    public class EmailLogic : IEmailLogic
    {
        private string ApiKey { get; }
        private string EmailAddress { get; }
        
        private string Username { get; }
        private string Password { get; }
        private string SmtpServer { get; }

        public EmailLogic(IOptions<SendGridSettings> sendGridSettings, IOptions<EmailSettings> emailSettings)
        {
            ApiKey = sendGridSettings.Value.ApiKey;
            EmailAddress = sendGridSettings.Value.EmailAddress;
            SmtpServer = emailSettings.Value.SmtpServer;
            Username = emailSettings.Value.Username;
            Password = emailSettings.Value.Password;
        }

        public async Task<bool> SendEmail(string email, string subject, string htmlContent)
        {
            var apiKey = ApiKey;
            var client = new SendGridClient(apiKey);
            
            var from = new EmailAddress(EmailAddress, "Support");
            var to = new EmailAddress(email);
            var plainTextContent = Regex.Replace(htmlContent, "<[^>]*>", "");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            
            return response.StatusCode == HttpStatusCode.Accepted;
        }

        public async Task<bool> SendEmailV2(string receiverEmail, string receiverName, string subject, string htmlContent)
        {
            var gmailSender = new GMailSender(Username, Password);
            var result = await gmailSender.SendAsync(subject, htmlContent, receiverEmail);
            
            //var email = new Mail
            //    {EmailTo = receiverEmail, NameTo = receiverName, Body = htmlContent, Subject = subject};
            //var a= await Emails.SendEmailAsync(email);
            
            
            return result;
        }
    }
}