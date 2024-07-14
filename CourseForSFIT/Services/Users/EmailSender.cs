using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Dtos.Models.EmailModels;

namespace Services.Users
{
    public interface IEmailSender
    {
        Task SendEmail(Email request);
    }
    public class EmailSender : IEmailSender
    {

        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmail(Email request)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetValue<string>("Email:EmailUsername")));
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetValue<string>("Email:EmailHost"), 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetValue<string>("Email:EmailUsername"), _config.GetValue<string>("Email:EmailPassword"));
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
