
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

        public async Task SendLoginEmailAsync(string to, string subject, string verifyUrl)
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
            string html = "<body " +
    "style=\"font-family: Arial, sans-serif;" +
    "background-color: #f4f4f4;" +
    "margin: 0;" +
    "padding: 0;" +
    "-webkit-text-size-adjust: none;" +
    "-ms-text-size-adjust: none;\">" +
    "<div style=\"max-width: 600px; margin: auto; background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\">" +

    "<div style=\"text-align: center; padding-bottom: 20px; color: #333333;\">" +
    "<h1 style=\"font-size: 24px; margin: 0; padding: 0;\">Kích hoạt tài khoản nhân viên</h1>" +
    "<p style=\"font-size: 16px; line-height: 1.5;\">Chào mừng bạn đến với hệ thống eMotoCare!</p>" +
    "<p style=\"font-size: 16px; line-height: 1.5;\">Vui lòng nhấn nút bên dưới để xác thực email và kích hoạt tài khoản của bạn.</p>" +

    "<a href=\"" + verifyUrl + "\" " +
    "style=\"display: inline-block; margin-top: 20px; padding: 15px 25px; font-size: 16px; color: #ffffff; background-color: #f52d56; border-radius: 5px; text-decoration: none;\">" +
    "Xác nhận tài khoản</a>" +

    "<p style=\"margin-top: 20px; font-size: 14px; color: #555555;\">" +
    "Nếu nút bên trên không hoạt động, vui lòng truy cập liên kết dưới đây:" +
    "</p>" +

    "<p style=\"font-size: 14px;\">" +
    "<a href=\"" + verifyUrl + "\" style=\"color: #1a0dab; text-decoration: underline;\">Xác thực tài khoản</a></p>" +

    "<p style=\"font-size: 14px; color: #777777; margin-top: 20px;\">" +
    "Nếu bạn không yêu cầu tạo tài khoản, vui lòng bỏ qua email này." +
    "</p>" +

    "<div style=\"text-align: center; font-size: 14px; color: #777777; margin-top: 30px;\">" +
    "<strong>Trân trọng,<br/>Đội ngũ eMotoCare</strong>" +
    "</div>" +
    "</div>" +
    "</body>";

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
