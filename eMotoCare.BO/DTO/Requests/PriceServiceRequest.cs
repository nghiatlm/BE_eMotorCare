using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Requests
{
    public class PriceServiceRequest
    {
        public Guid PartTypeId { get; set; }
        public Remedies Remedies { get; set; }
        public string Name { get; set; } = default!;
        public decimal LaborCost { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }
}
