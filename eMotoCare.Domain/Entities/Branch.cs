
using eMotoCare.Common.Enums;

using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Entities
{
    [Index(nameof(BranchName), IsUnique = true)]
    [Index(nameof(PhoneNumber), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Address), IsUnique = true)]
    public class Branch : BaseEntity
    {
        public Guid BranchId { get; set; }
        public Guid ManageById { get; set; }
        public Staff? ManageBy { get; set; }
        public Guid ServiceCenterId { get; set; }
        public virtual ServiceCenter? ServiceCenter { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Status Status { get; set; }

        public virtual ICollection<Staff>? Staffs { get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; }
        public virtual ICollection<ExportNote>? ExportNotes { get; set; }
        public virtual ICollection<ImportNote>? ImportNotes { get; set; }
        public virtual ICollection<BranchInventory>? BranchInventories { get; set; }

    }
}