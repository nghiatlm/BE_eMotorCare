
using eMotoCare.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Entities
{
    [Index(nameof(VinNumber), IsUnique = true)]
    [Index(nameof(EngineNumber), IsUnique = true)]
    public class Vehicle : BaseEntity
    {
        public Guid VehicleId { get; set; }
        public string VinNumber { get; set; }
        public string EngineNumber { get; set; }
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public Guid ModelId { get; set; }
        public Model? Model { get; set; }
        public string Manufacturer { get; set; }
        public DateTime ManufactureYear { get; set; }
        public string Color { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Status Status { get; set; }
        public string VehicleImage { get; set; }

        public virtual ICollection<VehicleStage>? VehicleStages { get; set; }
        public virtual ICollection<VehiclePartItem>? VehiclePartItems { get; set; }

    }
}