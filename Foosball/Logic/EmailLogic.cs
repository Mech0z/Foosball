using System.Net;
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
        private string ApiKey { get; set; }
        private string EmailAddress { get; set; }

        public EmailLogic(IOptions<SendGridSettings> sendGridSettings)
        {
            ApiKey = sendGridSettings.Value.ApiKey;
            EmailAddress = sendGridSettings.Value.EmailAddress;
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
    }
}