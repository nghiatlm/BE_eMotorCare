using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.Entities
{
    [Table("service_center")]
    public class ServiceCenter : BaseEntity
    {
        [Key]
        [Column("service_center_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("code", TypeName = "nvarchar(100)")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; } = string.Empty;

        [Column("description", TypeName = "nvarchar(400)")]
        public string? Description { get; set; }

        [Required]
        [Column("email", TypeName = "nvarchar(100)")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("phone", TypeName = "nvarchar(100)")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [Column("address", TypeName = "nvarchar(500)")]
        public string Address { get; set; } = string.Empty;

        [Column("latitude", TypeName = "nvarchar(300)")]
        public string? Latitude { get; set; }

        [Column("longitude", TypeName = "nvarchar(300)")]
        public string? Longitude { get; set; }

        [Required]
        [Column("status", TypeName = "varchar(200)")]
        [EnumDataType(typeof(StatusEnum))]
        public StatusEnum Status { get; set; }

        public virtual ICollection<ImportNote>? ImportNotes { get; set; }
        public virtual ICollection<ExportNote>? ExportNotes { get; set; }
        public virtual ICollection<Staff>? Staffs { get; set; }
        public virtual ICollection<ServiceCenterInventory>? ServiceCenterInventories { get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; }
    }
}