
using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class Vehicle : BaseEntity
    {
        public Guid Id { get; set; }
        public string VinNumber { get; set; }
        public string EngineNumber { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ModelId { get; set; }
        public string Manufacturer { get; set; }
        public DateTime ManufactureYear { get; set; }
        public string Color { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Status Status { get; set; }
        public string VehicleImage { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Model? Model { get; set; } 
    }
}