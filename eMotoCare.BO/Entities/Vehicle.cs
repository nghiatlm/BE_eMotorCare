
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("vehicle")]
    public class Vehicle
    {
        [Key]
        [Column("vehicle_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("model_id")]
        public Guid ModelId { get; set; }

        [ForeignKey(nameof(ModelId))]
        public Model? Model { get; set; }

        [Column("customer_id")]
        public Guid? CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }

        public virtual ICollection<VehicleStage>? VehicleStages { get; set; }
        public virtual ICollection<VehiclePartItem>? VehiclePartItems { get; set; }
    }
}