
using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class PriceService : BaseEntity
    {
        public Guid PriceServiceId { get; set; }
        public Guid PartTypeId { get; set; }
        public PartType? PartType { get; set; }
        public Remedies Remedies { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal LaborCost { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}