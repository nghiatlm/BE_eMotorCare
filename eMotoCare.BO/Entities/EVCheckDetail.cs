using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.Entities
{
    [Table("ev_check_detail")]
    public class EVCheckDetail
    {
        [Key]
        [Column("ev_check_detail_id")]
        public Guid Id { get; set; }

        [Column("maintenance_stage_detail_id")]
        public Guid? MaintenanceStageDetailId { get; set; }

        [ForeignKey(nameof(MaintenanceStageDetailId))]
        public virtual MaintenanceStageDetail? MaintenanceStageDetail { get; set; }

        [Column("campaign_detail_id")]
        public Guid? CampaignDetailId { get; set; }

        [ForeignKey(nameof(CampaignDetailId))]
        public virtual CampaignDetail? CampaignDetail { get; set; }

        [Required]
        [Column("part_item_id")]
        public Guid PartItemId { get; set; }

        [ForeignKey(nameof(PartItemId))]
        public virtual PartItem? PartItem { get; set; }

        [Required]
        [Column("ev_check_id")]
        public Guid EVCheckId { get; set; }

        [ForeignKey(nameof(EVCheckId))]
        public virtual EVCheck? EVCheck { get; set; }

        [Column("replace_part_id")]
        public Guid? ReplacePartId { get; set; }

        [ForeignKey(nameof(ReplacePartId))]
        [InverseProperty(nameof(PartItem.ReplcePart))]
        public virtual PartItem? ReplacePart { get; set; }

        [InverseProperty(nameof(RMADetail.EVCheckDetail))]
        public virtual RMADetail? RMADetail { get; set; }

        [InverseProperty(nameof(BatteryCheck.EVCheckDetail))]
        public virtual BatteryCheck? BatteryCheck { get; set; }

        [Column("result")]
        public string? Result { get; set; }

        [Required]
        [Column("remedies", TypeName = "varchar(200)")]
        public Remedies[] Remedies { get; set; }

        [Column("unit")]
        public string? Unit { get; set; }

        [Column("quantity", TypeName = "decimal(18,2)")]
        public decimal? Quantity { get; set; }

        [Column("price_part", TypeName = "decimal(18,2)")]
        public decimal? PricePart { get; set; }

        [Column("price_service", TypeName = "decimal(18,2)")]
        public decimal? PriceService { get; set; }

        [Column("total_amount", TypeName = "decimal(18,2)")]
        public decimal? TotalAmount { get; set; }

        [Required]
        [Column("status", TypeName = "varchar(200)")]
        [EnumDataType(typeof(EVCheckDetailStatus))]
        public EVCheckDetailStatus Status { get; set; }
    }
}
