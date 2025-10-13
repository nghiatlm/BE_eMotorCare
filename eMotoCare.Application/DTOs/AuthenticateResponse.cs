

namespace eMotoCare.Application.DTOs
{
    public class AuthenticateResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
