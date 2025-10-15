
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("customer")]
    public class Customer
    {
        [Key]
        [Column("customer_id")]
        public Guid Id { get; set; }

        [Column("account_id")]
        [Required]
        public Guid AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty(nameof(Account.Customer))]
        public Account? Account { get; set; }

        public virtual ICollection<Vehicle>? Vehilces { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; }
        public virtual ICollection<RMA>? RMAs { get; set; }

    }
}