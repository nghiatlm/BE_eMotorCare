using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class ModelRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public Guid MaintenancePlanId { get; set; }
    }
}
