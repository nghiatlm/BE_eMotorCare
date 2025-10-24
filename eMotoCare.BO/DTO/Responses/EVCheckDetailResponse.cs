
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Responses
{
    public class EVCheckDetailResponse
    {
        public Guid Id { get; set; }
        public MaintenanceStageDetailResponse? MaintenanceStageDetail { get; set; }
        public  CampaignDetail? CampaignDetail { get; set; }
        public PartItem? PartItem { get; set; }
        public EVCheckResponse? EVCheck { get; set; }
        public PartItem? ReplacePart { get; set; }
        public RMADetail? RMADetail { get; set; }
        public BatteryCheck? BatteryCheck { get; set; }
        public string? Result { get; set; }
        public Remedies Remedies { get; set; }
        public string? Unit { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? PricePart { get; set; }
        public decimal? PriceService { get; set; }
        public decimal? TotalAmount { get; set; }
        public Status Status { get; set; }
    }
}
