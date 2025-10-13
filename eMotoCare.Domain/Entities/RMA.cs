

using eMotoCare.Common.Enums;

namespace eMotoCare.DAL.Entities
{
    public class RMA : BaseEntity
    {
        public Guid RMAId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime RMADate{ get; set; }
        public RMAStatus RMAStatus { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Staff? CreatedBy { get; set; }
        public virtual ICollection<RMADetail>? RMADetails { get; set; }
    }
}