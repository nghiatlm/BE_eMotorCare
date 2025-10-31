

namespace eMotoCare.BO.DTO.Requests
{
    public class VerifyLoginRequest
    {
        public string Email { get; set; } = default!;
        public string Otp { get; set; } = default!;
    }
}
