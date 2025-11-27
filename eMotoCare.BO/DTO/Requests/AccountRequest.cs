using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class AccountRequest
    {
        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public RoleName RoleName { get; set; }
        public StaffRequest? Staff { get; set; }

        public AccountStatus Status { get; set; }
    }
}
