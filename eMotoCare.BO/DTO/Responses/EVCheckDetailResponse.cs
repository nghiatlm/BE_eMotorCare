
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Responses
{
    public class EVCheckDetailResponse
    {
        public Guid Id { get; set; }
        public MaintenanceStageDetailResponse? MaintenanceStageDetail { get; set; }
        public CampaignDetailResponse? CampaignDetail { get; set; }
        public PartItemResponse? PartItem { get; set; }
        public EVCheckResponse? EVCheck { get; set; }
        public PartItemResponse? ReplacePart { get; set; }
        public string? Result { get; set; }
        public Remedies[] Remedies { get; set; }
        public string? Unit { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? PricePart { get; set; }
        public decimal? PriceService { get; set; }
        public decimal? TotalAmount { get; set; }
        public EVCheckDetailStatus Status { get; set; }
    }
}
