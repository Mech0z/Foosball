using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            var fromAddress = new MailAddress(Username, "Foosball");
            var toAddress = new MailAddress(receiverEmail, receiverName);

            //https://stackoverflow.com/questions/32260/sending-email-in-net-through-gmail
            //Remember to turn ON access for unsafe apps on your gmail https://myaccount.google.com/lesssecureapps
            var smtp = new SmtpClient
            {
                Host = SmtpServer,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, Password)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = htmlContent
            })
            {
                await smtp.SendMailAsync(message);
            }

            return true;
        }
    }
}