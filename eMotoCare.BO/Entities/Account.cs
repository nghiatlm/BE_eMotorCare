
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;

namespace eMotoCare.BO.Entities
{
    [Table("account")]
    public class Account : BaseEntity
    {
        [Key]
        [Column("account_id")]
        public Guid Id { get; set; }

        [InverseProperty(nameof(Customer.Account))]
        public Customer? Customer { get; set; }

        [InverseProperty(nameof(Staff.Account))]
        public Staff? Staff { get; set; }
    }
}