using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Dtos.Models.EmailModels;
using Shared.Configs;
using Microsoft.Extensions.Options;

namespace Services.Users
{
    public interface IEmailSender
    {
        Task SendEmail(Email request);
    }
    public class EmailSender : IEmailSender
    {

        private readonly EmailSetting _serverMailSetting;

        public EmailSender(IOptions<EmailSetting> serverMailSetting)
        {
            _serverMailSetting = serverMailSetting.Value;
        }
        public async Task SendEmail(Email request)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_serverMailSetting.EmailUsername));
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

                using var smtp = new SmtpClient();
                smtp.Connect(_serverMailSetting.EmailHost, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_serverMailSetting.EmailUsername, _serverMailSetting.EmailPassword);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
