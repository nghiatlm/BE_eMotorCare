using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class EVCheckDetailItemRequest
    {
        [Required]
        public Guid PartItemId { get; set; }
        public Guid? MaintenanceStageDetailId { get; set; }
        public Guid? CampaignDetailId { get; set; }
        public Guid? ReplacePartId { get; set; }
        public string? Result { get; set; }

        [Required]
        public eMotoCare.BO.Enum.Remedies Remedies { get; set; }
        public string? Unit { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? PricePart { get; set; }
        public decimal? PriceService { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
