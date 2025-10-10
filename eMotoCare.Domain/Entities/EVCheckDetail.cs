
using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class EVCheckDetail : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid EVCheckId { get; set; }
        public Guid PartItemId { get; set; }
        public string CheckResult { get; set; }
        public Remedies Remedies { get; set; }
        public string SparePart { get; set; }
        public decimal PricePart { get; set; }
        public decimal PriceService { get; set; }
        public string Note { get; set; }
        public Status Status { get; set; }

        public virtual EVCheck? EVCheck { get; set; }
        public virtual PartItem? PartItem { get; set; }

    }
}