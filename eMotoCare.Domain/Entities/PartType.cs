
using eMotoCare.DAL.Entities;

namespace eMotoCare.DAL.Entities
{
    public class PartType : BaseEntity
    {
        public Guid PartTypeId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<ModelPartType>? ModelPartTypes { get; set; }
        public virtual ICollection<Part>? Parts { get; set; }
        public virtual ICollection<PriceService>? PriceServices { get; set; }
    }
}