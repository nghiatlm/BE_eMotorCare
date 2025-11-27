using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class VehicleRequest
    {
        public string Image { get; set; } = default!;
        public string Color { get; set; } = default!;
        public string ChassisNumber { get; set; } = default!;
        public string EngineNumber { get; set; } = default!;
        public StatusEnum Status { get; set; }
        public DateTime ManufactureDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime WarrantyExpiry { get; set; }
        public Guid ModelId { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
