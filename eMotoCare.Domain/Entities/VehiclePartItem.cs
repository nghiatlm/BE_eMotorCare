
using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class VehiclePartItem : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public Guid PartItemId { get; set; }
        public DateTime InstalledDate { get; set; }
        public Status Status { get; set; }
        public Guid ReplaceForId { get; set; }

        public virtual Vehicle? Vehicle { get; set; }
        public virtual PartItem? PartItem { get; set; }


    }
}