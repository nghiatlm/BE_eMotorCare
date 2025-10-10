

using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class MaintenaceStageDetail : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid MaintenaceStageId { get; set; }
        public Guid PartId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public ServiceTask ServiceTask { get; set; }
        public string Description { get; set; } 

        public virtual MaintenaceStage? MaintenaceStage { get; set; }
        public virtual Part? Part { get; set; }

    }
}