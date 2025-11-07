
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.Entities
{
    [Table("customer")]
    public class Customer : BaseEntity
    {
        [Key]
        [Column("customer_id")]
        public Guid Id { get; set; }
        [Column("customer_code", TypeName = "varchar(100)")]
        public string? CustomerCode { get; set; }

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

        [Column("account_id")]
        [Required]
        public Guid AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty(nameof(Account.Customer))]
        public Account? Account { get; set; }

        public virtual ICollection<Vehicle>? Vehilces { get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; }

    }
}