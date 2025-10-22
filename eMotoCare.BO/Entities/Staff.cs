using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.Entities
{
    [Table("staff")]
    public class Staff : BaseEntity
    {
        [Key]
        [Column("staff_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("staff_code", TypeName = "nvarchar(100)")]
        public string StaffCode { get; set; } = string.Empty;

        [Column("first_name", TypeName = "nvarchar(300)")]
        public string? FirstName { get; set; }

        [Column("last_name", TypeName = "nvarchar(300)")]
        public string? LastName { get; set; }

        [Column("address", TypeName = "nvarchar(400)")]
        public string? Address { get; set; }

        [Required]
        [Column("citizen_id", TypeName = "varchar(15)")]
        public string CitizenId { get; set; } = string.Empty;

        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        [Column("gender", TypeName = "varchar(200)")]
        [EnumDataType(typeof(GenderEnum))]
        public GenderEnum? Gender { get; set; }

        [Column("avatar_url")]
        public string? AvatarUrl { get; set; }

        [Required]
        [Column("position", TypeName = "varchar(200)")]
        [EnumDataType(typeof(PositionEnum))]
        public PositionEnum Position { get; set; }

        [Column("account_id")]
        [Required]
        public Guid AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty(nameof(Account.Staff))]
        public Account? Account { get; set; }

        [Required]
        [Column("service_center_id")]
        public Guid ServiceCenterId { get; set; }

        [ForeignKey(nameof(ServiceCenterId))]
        public virtual ServiceCenter? ServiceCenter { get; set; }
        public virtual ICollection<RMA>? RMAs { get; set; } //note
        public virtual ICollection<EVCheck>? EVChecks { get; set; } //note
        public virtual ICollection<Appointment>? Appointments { get; set; } //note
        public virtual ICollection<ExportNote>? ExportNotes { get; set; } //note
        public virtual ICollection<ImportNote>? ImportNotes { get; set; } //note
    }
}
