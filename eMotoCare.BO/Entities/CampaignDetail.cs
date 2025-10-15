
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Required]
        [Column("part_id")]
        public Guid PartId { get; set; }

        [ForeignKey(nameof(PartId))]
        public virtual Part? Part { get; set; }

        public virtual ICollection<EVCheckDetail>? EVCheckDetails { get; set; }
    }
}