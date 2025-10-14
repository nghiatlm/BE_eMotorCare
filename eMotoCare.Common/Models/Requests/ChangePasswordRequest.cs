

using System.ComponentModel.DataAnnotations;

namespace eMotoCare.Common.Models.Requests
{
    public class ChangePasswordRequest
    {
        [Required]
        public string? OldPassword { get; set; }
        [Required]
        public string? NewPassword { get; set; }
        [Required]
        public string? ConfirmPassword { get; set; }
    }
}
