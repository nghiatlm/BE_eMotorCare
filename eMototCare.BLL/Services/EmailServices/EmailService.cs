
using eMotoCare.BO.Common;
using MimeKit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace eMototCare.BLL.Services.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<MailSettings> settings, ILogger<EmailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task SendLoginEmailAsync(string to, string subject, string otpCode)
        {
            var fromName = Environment.GetEnvironmentVariable("MailSettings__FromName")
                      ?? _settings.FromName;
            var fromEmail = Environment.GetEnvironmentVariable("MailSettings__FromEmail")
                            ?? _settings.FromEmail;
            var password = Environment.GetEnvironmentVariable("MailSettings__Password")
                            ?? _settings.Password;
            var host = Environment.GetEnvironmentVariable("MailSettings__Host")
                            ?? _settings.Host;
            var portEnv = Environment.GetEnvironmentVariable("MailSettings__Port");
            int port = 0;
            if (!string.IsNullOrEmpty(portEnv) && int.TryParse(portEnv, out var p))
                port = p;
            else
                port = _settings.Port;
            string html = $@"
        <html>
            <body style='font-family:Arial,sans-serif; color:#333;'>
                <h2 style='color:#0078D4;'>Xác thực đăng nhập</h2>
                <p>Xin chào,</p>
                <p>Bạn vừa yêu cầu đăng nhập vào hệ thống eMotoCare.</p>
                <p>Mã OTP của bạn là:</p>
                <h1 style='letter-spacing:3px; color:#0078D4;'>{otpCode}</h1>
                <p>Mã này có hiệu lực trong 5 phút.</p>
                <br/>
                <p>Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này.</p>
                <p>Trân trọng,<br/><b>eMotoCare Team</b></p>
            </body>
        </html>";
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(fromName, fromEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = html
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(fromEmail, password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation("Sent email to {Email}", to);
        }
    }
}
