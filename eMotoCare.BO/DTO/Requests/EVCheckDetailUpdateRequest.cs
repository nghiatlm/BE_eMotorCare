

using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Requests
{
    public class EVCheckDetailUpdateRequest
    {
        public Guid? MaintenanceStageDetailId { get; set; }
        public Guid? CampaignDetailId { get; set; }
        public Guid? PartItemId { get; set; }
        public Guid? EVCheckId { get; set; }
        public Guid? ProposedReplacePartId { get; set; }
        public string? Result { get; set; }
        public Remedies? Remedies { get; set; }
        public string? Unit { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? PricePart { get; set; }
        public decimal? PriceService { get; set; }
        public decimal? TotalAmount { get; set; }
        [Required]
        [EnumDataType(typeof(EVCheckDetailStatus))]
        public EVCheckDetailStatus? Status { get; set; }
        
    }
}
