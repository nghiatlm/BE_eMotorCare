
using eMotoCare.Common.Enums;

namespace eMotoCare.DAL.Entities
{
    public class VehiclePartItem : BaseEntity
    {
        public Guid VehiclePartItemId { get; set; }
        public Guid VehicleId { get; set; }
        public Guid PartItemId { get; set; }
        public DateTime InstalledDate { get; set; }
        public Status Status { get; set; }
        public Guid? ReplaceForId { get; set; }

        public Vehicle? Vehicle { get; set; }
        public PartItem? PartItem { get; set; }
        public PartItem? ReplaceFor { get; set; }



    }
}