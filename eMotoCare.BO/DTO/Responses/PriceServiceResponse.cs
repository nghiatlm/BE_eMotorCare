using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Responses
{
    public class PriceServiceResponse
    {
        public Guid Id { get; set; }
        public Guid PartTypeId { get; set; }
        public string Code { get; set; } = default!;
        public Remedies Remedies { get; set; }
        public string Name { get; set; } = default!;
        public decimal LaborCost { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? PartTypeName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
