using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("rma_detail")]
    public class RMADetail
    {
        [Key]
        [Column("rma_detail_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("part_item_id")]
        public Guid PartItemId { get; set; }

        [ForeignKey(nameof(PartItemId))]
        public virtual PartItem? PartItem { get; set; }

        [Required]
        [Column("ev_check_detail_id")]
        public Guid EVCheckDetailId { get; set; }

        [ForeignKey(nameof(EVCheckDetailId))]
        public virtual EVCheckDetail? EVCheckDetail { get; set; }

        [Required]
        [Column("rma_id")]
        public Guid RMAId { get; set; }

        [ForeignKey(nameof(RMAId))]
        public virtual RMA? RMA { get; set; }
    }
}