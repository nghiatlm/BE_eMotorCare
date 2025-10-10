

using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class RMA : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime RMADate{ get; set; }
        public RMAStatus RMAStatus { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Staff? CreatedBy { get; set; }

    }
}