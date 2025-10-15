using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("service_center")]
    public class ServiceCenter
    {
        [Key]
        [Column("service_center_id")]
        public Guid Id { get; set; }

        public virtual ICollection<ImportNote>? ImportNotes { get; set; }
        public virtual ICollection<ExportNote>? ExportNotes { get; set; }
        public virtual ICollection<Staff>? Staffs { get; set; }
        public virtual ICollection<ServiceCenterInventory>? ServiceCenterInventories { get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; }
    }
}