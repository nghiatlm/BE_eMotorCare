using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Responses
{
    public class VehicleResponse
    {
        public Guid Id { get; set; }
        public string Image { get; set; } = default!;
        public string Color { get; set; } = default!;
        public string ChassisNumber { get; set; } = default!;
        public string EngineNumber { get; set; } = default!;
        public StatusEnum Status { get; set; }
        public DateTime ManufactureDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime WarrantyExpiry { get; set; }
        public Guid ModelId { get; set; }
        public string? ModelName { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
