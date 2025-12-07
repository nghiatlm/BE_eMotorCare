
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
            string html = "<body \n" +
                    "    style=\"font-family: Arial, sans-serif;\n" +
                    "            background-color: #f4f4f4;\n" +
                    "            margin: 0;\n" +
                    "            padding: 0;\n" +
                    "            -webkit-text-size-adjust: none;\n" +
                    "            -ms-text-size-adjust: none;\">\n" +
                    "    <div class=\"email-container\"\n" +
                    "         style=\"max-width: 600px;\n" +
                    "                margin: auto;\n" +
                    "                background-color: #ffffff;\n" +
                    "                padding: 20px;\n" +
                    "                border-radius: 8px;\n" +
                    "                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\">\n" +
                    "        <div class=\"header\"\n" +
                    "             style=\"text-align: center;\n" +
                    "                    padding-bottom: 20px;\">\n" +
                    "            <img src=\"https://firebasestorage.googleapis.com/v0/b/diamond-6401b.appspot.com/o/Logo.png?alt=media&token=13f983ed-b3e1-4bbe-83b2-a47edf62c6a6\"\n" +
                    "                alt=\"Logo\" style=\"max-width: 300px;\">\n" +
                    "        </div>\n" +
                    "        <div class=\"content\"\n" +
                    "              style=\"text-align: center;\n" +
                    "                    color: #333333;\">\n" +
                    "            <h1\n" +
                    "            style=\"font-size: 24px;\n" +
                    "                margin: 0;\n" +
                    "                padding: 0;\"\n" +
                    "            >Verify your email address</h1>\n" +
                    "            <p\n" +
                    "            style=\"font-size: 16px;\n" +
                    "                    line-height: 1.5;\">Welcome to Modern Estate.</p>\n" +
                    "            <p\n" +
                    "            style=\"font-size: 16px;\n" +
                    "                line-height: 1.5;\">Please click the button below to confirm your email address and activate your account.</p>\n" +
                    "            <a href=\"" + verifyUrl + "\" class=\"btn\"\n" +
                    "               style=\"display: inline-block;\n" +
                    "               margin-top: 20px;\n" +
                    "               padding: 15px 25px;\n" +
                    "               font-size: 16px;\n" +
                    "               color: #ffffff;\n" +
                    "               background-color: #f52d56;\n" +
                    "               border-radius: 5px;\n" +
                    "               text-decoration: none;\">Confirm Email</a>\n" +
                    "<p style=\"margin-top: 20px; font-size: 14px; color: #555555;\">" +
                    "If the button doesn't work, please press the link below:\n" +
                    "</p>\n" +
                    "<p style=\"font-size: 14px; color: #1a0dab;\">\n" +
                    "    <a href=\"" + verifyUrl + "\" style=\"color: #1a0dab; text-decoration: underline;\">Verify your account</a>\n" +
                    "</p>" +
                    "            <p>If you received this email in error, simply ignore this email and do not click the button.</p>\n" +
                    "        </div>\n" +
                    "        <div class=\"footer\"\n" +
                    "             style=\"text-align: center;\n" +
                    "             font-size: 14px;\n" +
                    "             color: #777777;\n" +
                    "             margin-top: 20px;\">\n" +
                    "            <h2>Thank you, have a good day .</h2></br>Modern Estate Team\n" +
                    "        </div>\n" +
                    "    </div>\n" +
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
