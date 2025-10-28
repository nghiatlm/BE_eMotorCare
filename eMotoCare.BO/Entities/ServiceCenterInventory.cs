
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("service_center_inventory")]
    public class ServiceCenterInventory
    {
        [Key]
        [Column("service_center_inventory_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("service_center_id")]
        public Guid ServiceCenterId { get; set; }

        [ForeignKey(nameof(ServiceCenterId))]
        public virtual ServiceCenter? ServiceCenter { get; set; }
        public virtual ICollection<PartItem>? PartItems { get; set; }
    }
}