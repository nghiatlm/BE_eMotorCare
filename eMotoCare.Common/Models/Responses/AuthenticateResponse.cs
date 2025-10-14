namespace eMotoCare.Common.Models.Responses
{
    public class AuthenticateResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
