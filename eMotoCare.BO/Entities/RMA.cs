
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("rma")]
    public class RMA
    {
        [Key]
        [Column("rma_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("create_by_id")]
        public Guid CreateById { get; set; }

        [ForeignKey(nameof(CreateById))]
        public virtual Staff? Staff { get; set; }

        [Required]
        [Column("customer_id")]
        public Guid CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer? Customer { get; set; }

        public virtual ICollection<RMADetail>? RMADetails { get; set; }

    }
}