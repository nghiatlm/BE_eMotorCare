using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class VehiclePartItemRequest
    {
        [Required]
        public DateTime InstallDate { get; set; }

        [Required]
        public Guid VehicleId { get; set; }

        [Required]
        public Guid PartItemId { get; set; }
        public Guid? ReplaceForId { get; set; }
    }
}
