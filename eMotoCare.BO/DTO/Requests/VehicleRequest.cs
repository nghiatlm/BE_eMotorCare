using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class VehicleRequest
    {


        [Required]
        public string Image { get; set; } = default!;

        [Required]
        public string Color { get; set; } = default!;

        [Required]
        public string ChassisNumber { get; set; } = default!;

        [Required]
        public string EngineNumber { get; set; } = default!;

        [Required]
        public StatusEnum Status { get; set; }

        [Required]
        public DateTime ManufactureDate { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public DateTime WarrantyExpiry { get; set; }

        [Required]
        public Guid ModelId { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
