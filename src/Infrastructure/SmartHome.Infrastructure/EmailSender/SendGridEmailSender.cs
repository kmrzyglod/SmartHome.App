using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using SmartHome.Application.Interfaces.EmailSender;

namespace SmartHome.Infrastructure.EmailSender
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly EmailAddress _from = new EmailAddress("smarthome@kmrzyglod.pl", "KM Smart Home Notifications");
        private readonly ISendGridClient _sendGridClient;

        public SendGridEmailSender(ISendGridClient sendGridClient)
        {
            _sendGridClient = sendGridClient;
        }

        public Task SendPlainText(string recipientEmail, string subject, string content)
        {
            return SendEmail(recipientEmail, subject, content, string.Empty);
        }

        public Task SendHtml(string recipientEmail, string subject, string content)
        {
            return SendEmail(recipientEmail, subject, string.Empty, content);
        }

        private async Task SendEmail(string recipientEmail, string subject, string? plainTextContent = null,
            string? htmlContent = null)
        {
            var to = new EmailAddress(recipientEmail);
            var msg = MailHelper.CreateSingleEmail(_from, to, subject, plainTextContent, htmlContent);
            var response = await _sendGridClient.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"[SendGridEmailSender] Error during sending email. Status code: {response.StatusCode}. Email recipient: {recipientEmail} Email subject: {subject}. Email content: {plainTextContent ?? htmlContent}");
            }
        }
    }
}