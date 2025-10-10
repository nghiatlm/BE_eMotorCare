
using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class Account : BaseEntity
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public RoleName Role { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? StaffId { get; set; }


        public  virtual Customer? Customer { get; set; }
        public  virtual Staff? Staff { get; set; }

    }
}