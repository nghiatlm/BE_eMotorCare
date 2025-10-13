
using eMotoCare.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Phone), IsUnique = true)]
    public class Account : BaseEntity
    {
        public Guid AccountId { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public RoleName Role { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? StaffId { get; set; }


        public  Customer? Customer { get; set; }
        public  Staff? Staff { get; set; }

    }
}