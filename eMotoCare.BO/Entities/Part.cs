
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;

namespace eMotoCare.BO.Entities
{
    [Table("part")]
    public class Part : BaseEntity
    {
        [Key]
        [Column("part_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("part_type_id")]
        public Guid PartTypeId { get; set; }

        [ForeignKey(nameof(PartTypeId))]
        public virtual PartType? PartType { get; set; }

        public virtual ICollection<PartItem>? PartItems { get; set; }
        public virtual ICollection<CampaignDetail>? CampaignDetails { get; set; }
        public virtual ICollection<MaintenanceStageDetail>? MaintenanceStageDetails { get; set; }
    }
}