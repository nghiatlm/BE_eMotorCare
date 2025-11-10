
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.Entities
{
    [Table("rma")]
    public class RMA : BaseEntity
    {
        [Key]
        [Column("rma_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("code", TypeName = "nvarchar(100)")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [Column("rma_date")]
        public DateTime RMADate { get; set; }

        [Required]
        [Column("return_address")]
        public string ReturnAddress { get; set; } = string.Empty;

        [Required]
        [Column("status", TypeName = "varchar(200)")]
        [EnumDataType(typeof(RMAStatus))]
        public RMAStatus Status { get; set; }

        [Column("note")]
        public string? Note { get; set; }
        [Required]
        [Column("customer_id")]
        public Guid CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public virtual Customer? Customer { get; set; }

        [Required]
        [Column("create_by_id")]
        public Guid CreateById { get; set; }

        [ForeignKey(nameof(CreateById))]
        public virtual Staff? Staff { get; set; }

        public virtual ICollection<RMADetail>? RMADetails { get; set; }

    }
}