

using eMotoCare.Common.Enums;

namespace eMotoCare.Common.Models.Responses
{
    public class AccountCurrent
    {
        public string? AccountId { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public RoleName Role { get; set; }

    }
}
