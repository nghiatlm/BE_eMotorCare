
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Responses
{
    public class AccountResponse
    {
        public Guid Id { get; set; }

        public string Phone { get; set; } = string.Empty;

        public string? Email { get; set; }
        public AccountStatus Stattus { get; set; }
        public RoleName RoleName { get; set; }
        // public Customer? Customer { get; set; }
        // public Staff? Staff { get; set; }
    }
}