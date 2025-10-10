
using eMotoCare.Domain.Common;

namespace eMotoCare.Domain.Entities
{
    public class RMADetail : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid EVCheckDetailId { get; set; }
        public Guid PartItemId { get; set; }
        public Guid RMAId { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
        public string RMANumber { get; set; }
        public DateTime ReleaseDateRMA { get; set; }
        public DateTime ExpirationDateRMA { get; set; }
        public string ReturnAddress { get; set; }
        public string Inspector { get; set; }
        public string CheckResult { get; set; }
        public string Solution { get; set; }
        public Guid ReplacePartId { get; set; }

        public virtual RMA? RMA { get; set; }
        public virtual EVCheckDetail? EVCheckDetail { get; set; }
        public virtual PartItem? PartItem { get; set; }


    }
}