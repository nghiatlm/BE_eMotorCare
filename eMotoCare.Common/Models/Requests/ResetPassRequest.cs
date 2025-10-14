
namespace eMotoCare.Common.Models.Requests
{
    public class ResetPassRequest
    {
        public string PhoneNumber { get; set; } = null!;
        public string Otp { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }
}
