

using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.Domain.Entities
{
    [Index(nameof(CitizenId), IsUnique = true)]
    [Index(nameof(StaffCode), IsUnique = true)]
    public class Staff : BaseEntity
    {
        public Guid StaffId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string CitizenId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public StaffPosition StaffPosition { get; set; }
        public Guid BranchId { get; set; }
        public Branch? Branch { get; set; }
        public string StaffCode { get; set; }
        public string? Avatar { get; set; }

        public virtual ICollection<Appointment>? Appointments { get; set; }
        public virtual ICollection<EVCheck>? EVChecks { get; set; }

    }
}