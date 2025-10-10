

using eMotoCare.Domain.Common;

namespace eMotoCare.Domain.Entities
{
    public class Part : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid PartTypeId { get; set; }
        public string PartName { get; set; }
        public string PartCode { get; set; }
        public string Manufacturer { get; set; }
        public int Quantity { get; set; }
        public string Origin { get; set; }
        public string Description { get; set; }
        
        public virtual PartType? PartType { get; set; }
    }
}