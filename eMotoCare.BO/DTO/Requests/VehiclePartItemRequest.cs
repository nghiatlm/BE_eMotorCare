using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class VehiclePartItemRequest
    {
        public DateTime InstallDate { get; set; }
        public Guid VehicleId { get; set; }
        public Guid PartItemId { get; set; }
        public Guid? ReplaceForId { get; set; }
    }
}
