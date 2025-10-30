using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.Entities
{
    [Table("account")]
    public class Account : BaseEntity
    {
        [Key]
        [Column("account_id")]
        public Guid Id { get; set; }

        [Column("phone", TypeName = "varchar(15)")]
        public string Phone { get; set; } = string.Empty;

        [Column("email", TypeName = "varchar(200)")]
        public string? Email { get; set; }

        [Required]
        [Column("password", TypeName = "varchar(200)")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Column("status", TypeName = "varchar(200)")]
        [EnumDataType(typeof(AccountStatus))]
        public AccountStatus Stattus { get; set; }

        [Required]
        [Column("role_ame", TypeName = "varchar(200)")]
        [EnumDataType(typeof(RoleName))]
        public RoleName RoleName { get; set; }

        [InverseProperty(nameof(Customer.Account))]
        public Customer? Customer { get; set; }

        [InverseProperty(nameof(Staff.Account))]
        public Staff? Staff { get; set; }
    }
}
