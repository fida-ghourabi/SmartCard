using MailKit.Net.Smtp;
using MimeKit;
using STB.SmartCard.Application.Services.Email;

namespace STB.SmartCard.Infrastructure.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp-relay.brevo.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "946a3e002@smtp-brevo.com"; // Ta clé API ici
        private readonly string _smtpPass = "EWZkPf0HBy2TmAjh"; // Oui, même valeur que user

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("STB SmartCard", "fidagh803@gmail.com"));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpUser, _smtpPass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
