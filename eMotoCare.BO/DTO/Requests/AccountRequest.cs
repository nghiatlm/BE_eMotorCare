using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class AccountRequest
    {
        [Required]
        public string? FullName { get; set; }
        public string? Phone { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Password { get; set; }

        [Required]
        public RoleName RoleName { get; set; }

        [Required]
        public AccountStatus Status { get; set; }
    }
}
