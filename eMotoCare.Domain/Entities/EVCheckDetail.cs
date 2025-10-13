
using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class EVCheckDetail : BaseEntity
    {
        public Guid EVCheckDetailId { get; set; }
        public Guid EVCheckId { get; set; }
        public EVCheck? EVCheck { get; set; }
        public Guid PartItemId { get; set; }
        public PartItem? PartItem { get; set; }
        public string CheckResult { get; set; }
        public Remedies Remedies { get; set; }
        public string SparePart { get; set; } //Cần xem lại kiểu dữ liệu
        public decimal PricePart { get; set; }
        public decimal PriceService { get; set; }
        public string Note { get; set; }
        public Status Status { get; set; }
    }
}