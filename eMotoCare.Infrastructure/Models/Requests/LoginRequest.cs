using System.ComponentModel.DataAnnotations;

namespace eMotoCare.Common.Models.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
