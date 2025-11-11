

using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class ServiceCenterInventoryRequest
    {

        [Required]
        public Guid ServiceCenterId { get; set; }
        [Required]
        public string ServiceCenterInventoryName { get; set; }

    }
}
