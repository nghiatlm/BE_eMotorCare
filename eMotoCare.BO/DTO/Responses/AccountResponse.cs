using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Responses
{
    public class AccountResponse
    {
        public Guid Id { get; set; }
        public string Phone { get; set; } = string.Empty;

        public string? Email { get; set; }
        public AccountStatus Status { get; set; }
        public RoleName RoleName { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CustomerResponse? Customer { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public StaffResponse? Staff { get; set; }
        //public object Stattus { get; set; }
        // public Customer? Customer { get; set; }
        // public Staff? Staff { get; set; }
    }
}
