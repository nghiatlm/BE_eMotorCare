

namespace eMotoCare.Common.Models.Responses
{
    public class ForgetPasswordResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public static ForgetPasswordResponse Fail(string message) => new() { Success = false, Message = message };
        public static ForgetPasswordResponse Ok(string message = "") => new() { Success = true, Message = message };
    }
}
