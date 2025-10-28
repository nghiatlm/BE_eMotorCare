using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Responses
{
    public class AccountResponse
    {
        public Guid Id { get; set; }

        [Required]
        public string? FullName { get; set; }
        public string Phone { get; set; } = string.Empty;

        public string? Email { get; set; }
        public AccountStatus Stattus { get; set; }
        public RoleName RoleName { get; set; }
        public CustomerResponse? Customer { get; set; }
        public StaffResponse? Staff { get; set; }
        // public Customer? Customer { get; set; }
        // public Staff? Staff { get; set; }
    }
}
