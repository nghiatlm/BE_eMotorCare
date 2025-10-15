
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("battery_check")]
    public class BatteryCheck
    {
        [Key]
        [Column("battery_check_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("ev_check_detail_id")]
        public Guid EVCheckDetailId { get; set; }

        [ForeignKey(nameof(EVCheckDetailId))]
        public virtual EVCheckDetail? EVCheckDetail { get; set; }

        [Required]
        [Column("part_item_id")]
        public Guid PartItemId { get; set; }

        [ForeignKey(nameof(PartItemId))]
        public virtual PartItem? PartItem { get; set; }
    }
}