
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.Entities
{
    [Table("vehicle")]
    public class Vehicle : BaseEntity
    {
        [Key]
        [Column("vehicle_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("image")]
        public string Image { get; set; } = string.Empty;

        [Required]
        [Column("color", TypeName = "nvarchar(300)")]
        public string Color { get; set; } = string.Empty;

        [Required]
        [Column("chassis_number", TypeName = "nvarchar(300)")]
        public string ChassisNumber { get; set; } = string.Empty;

        [Required]
        [Column("engine_number", TypeName = "nvarchar(300)")]
        public string EngineNumber { get; set; } = string.Empty;

        [Required]
        [Column("status", TypeName = "varchar(200)")]
        [EnumDataType(typeof(StatusEnum))]
        public StatusEnum Status { get; set; }

        [Required]
        [Column("manufacture_date")]
        public DateTime ManufactureDate { get; set; }

        [Required]
        [Column("purchase_date")]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [Column("warranty_expiry")]
        public DateTime WarrantyExpiry { get; set; }

        [Required]
        [Column("model_id")]
        public Guid ModelId { get; set; }

        [ForeignKey(nameof(ModelId))]
        public Model? Model { get; set; }

        [Column("customer_id")]
        public Guid? CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }
        [Required]
        [Column("is_primary")]
        public bool IsPrimary { get; set; } = false;
        public virtual ICollection<VehicleStage>? VehicleStages { get; set; }
        public virtual ICollection<VehiclePartItem>? VehiclePartItems { get; set; }
    }
}