
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;

namespace eMotoCare.BO.Entities
{
    [Table("vehicle_part_item")]
    public class VehiclePartItem : BaseEntity
    {
        [Key]
        [Column("vehicle_part_item_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("install_date")]
        public DateTime InstallDate { get; set; }

        [Required]
        [Column("vehicle_id")]
        public Guid VehicleId { get; set; }

        [ForeignKey(nameof(VehicleId))]
        public virtual Vehicle? Vehicle { get; set; }

        [Required]
        [Column("part_item_id")]
        public Guid PartItemId { get; set; }

        [ForeignKey(nameof(PartItemId))]
        public virtual PartItem? PartItem { get; set; }

        [Column("replace_for_id")]
        public Guid? ReplaceForId { get; set; }

        [ForeignKey(nameof(ReplaceForId))]
        public virtual PartItem? ReplaceFor { get; set; }
    }
}