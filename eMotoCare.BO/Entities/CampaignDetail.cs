using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Enum;

namespace eMotoCare.BO.Entities
{
    [Table("campaign_detail")]
    public class CampaignDetail
    {
        [Key]
        [Column("campaign_detail_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("campaign_id")]
        public Guid CampaignId { get; set; }

        [ForeignKey(nameof(CampaignId))]
        public virtual Campaign? Campaign { get; set; }

        public string Description { get; set; }
        public Guid EVCheckDetailId { get; set; }

        [Required]
        [Column("part_id")]
        public Guid PartId { get; set; }

        [ForeignKey(nameof(PartId))]
        public virtual Part? Part { get; set; }

        [Required]
        [Column("action_type")]
        public CampaignActionType ActionType { get; set; }

        [Column("note")]
        [StringLength(2000)]
        public string? Note { get; set; }

        [Required]
        [Column("is_mandatory")]
        public bool IsMandatory { get; set; }

        [Column("estimated_time")]
        public int? EstimatedTime { get; set; }

        public virtual ICollection<EVCheckDetail>? EVCheckDetails { get; set; }
    }
}
