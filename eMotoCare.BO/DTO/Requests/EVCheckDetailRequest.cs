

using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class EVCheckDetailRequest
    {
        public Guid? MaintenanceStageDetailId { get; set; }
        public Guid? CampaignDetailId { get; set; }
        public Guid PartItemId { get; set; }
        public Guid EVCheckId { get; set; }
        public Guid? ReplacePartId { get; set; }
        public string? Result { get; set; }
        public Remedies[] Remedies { get; set; }
        public string? Unit { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? PricePart { get; set; }
        public decimal? PriceService { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
