using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("staff")]
    public class Staff
    {
        [Key]
        [Column("staff_id")]
        public Guid Id { get; set; }


        [Column("account_id")]
        [Required]
        public Guid AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty(nameof(Account.Staff))]
        public Account? Account { get; set; }

        public virtual ICollection<RMA>? RMAs { get; set; } //note
        public virtual ICollection<EVCheck>? EVChecks { get; set; } //note
        public virtual ICollection<Appointment>? Appointments { get; set; } //note
        public virtual ICollection<ExportNote>? ExportNotes { get; set; } //note
        public virtual ICollection<ImportNote>? ImportNotes { get; set; } //note

    }
}