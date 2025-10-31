

namespace eMotoCare.BO.Common
{
    public class MailSettings
    {
        public string FromEmail { get; set; } = default!;
        public string FromName { get; set; } = "eMotoCare";
        public string Password { get; set; } = default!;
        public string Host { get; set; } = "smtp.office365.com";
        public int Port { get; set; } = 587;
    }
}
