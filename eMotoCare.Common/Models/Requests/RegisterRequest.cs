namespace eMotoCare.Common.Models.Requests
{
    public class RegisterRequest
    {
        public string Phone { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
    }
}
