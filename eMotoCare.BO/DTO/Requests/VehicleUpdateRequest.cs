using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class VehicleUpdateRequest
    {
        public string? Image { get; set; } = default!;
        public string? Color { get; set; } = default!;
        public StatusEnum? Status { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyExpiry { get; set; }
        public Guid? ModelId { get; set; }
        public Guid? CustomerId { get; set; }
        public bool? IsPrimary { get; set; } = false;
    }
}
