
using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class Branch : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid ManagerById { get; set; }
        public Guid ServiceCenterId { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Status Status { get; set; }

        public virtual Staff? ManagerBy { get; set; }
        public virtual ServiceCenter? ServiceCenter { get; set; }

    }
}