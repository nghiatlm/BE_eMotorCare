
using eMotoCare.DAL.Entities;

namespace eMotoCare.DAL.Entities
{
    public class ServiceCenter : BaseEntity
    {
        public Guid ServiceCenterId { get; set; }
        public string CenterName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Branch>? Branches { get; set; }

    }
}