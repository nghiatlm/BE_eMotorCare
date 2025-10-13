

using eMotoCare.DAL.Entities;
using eMotoCare.Common.Enums;

namespace eMotoCare.DAL.Entities
{
    public class MaintenaceStageDetail : BaseEntity
    {
        public Guid MaintenaceStageDetailId { get; set; }
        public Guid MaintenaceStageId { get; set; }
        public Guid PartId { get; set; }
        public string Name { get; set; }
        public ServiceTask ServiceTask { get; set; }
        public string Description { get; set; } 

        public MaintenaceStage? MaintenaceStage { get; set; }
        public Part? Part { get; set; }
        public virtual ICollection<VehicleStage>? VehicleStages { get; set; }

    }
}