using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eMotoCare.Common.Enums;

namespace eMotoCare.Common.Models.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string FullName { get; set; } = string.Empty;
        public RoleName Role { get; set; }
        public string? AvatarUrl { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
