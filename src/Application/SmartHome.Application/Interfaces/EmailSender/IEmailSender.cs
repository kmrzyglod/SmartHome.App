using System.Threading.Tasks;

namespace SmartHome.Application.Interfaces.EmailSender
{
    public interface IEmailSender
    {
        Task SendPlainText(string recipientEmail, string subject, string content);
        Task SendHtml(string recipientEmail, string subject, string content);
    }
}