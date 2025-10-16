using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;

namespace eMotoCare.BO.Entities
{
    [Table("rma_detail")]
    public class RMADetail : BaseEntity
    {
        [Key]
        [Column("rma_detail_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("quantity", TypeName = "int")]
        public int Quantity { get; set; }

        [Column("reason", TypeName = "nvarchar(400)")]
        public string? Reason { get; set; }

        [Required]
        [Column("rma_number", TypeName = "varchar(100)")]
        public string RMANumber { get; set; } = string.Empty;

        [Column("release_date_rma")]
        public DateTime? ReleaseDateRMA { get; set; }

        [Column("expiration_date_rma")]
        public DateTime? ExpirationDateRMA { get; set; }

        [Column("inspector", TypeName = "nvarchar(400)")]
        public string? Inspector { get; set; }

        [Column("result", TypeName = "nvarchar(400)")]
        public string? Result { get; set; }

        [Column("solution", TypeName = "nvarchar(400)")]
        public string? Solution { get; set; }

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