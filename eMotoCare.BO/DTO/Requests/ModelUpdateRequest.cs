using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Requests
{
    public class ModelUpdateRequest
    {
        public string? Name { get; set; } = string.Empty;

        public string? Manufacturer { get; set; } = string.Empty;

        public Guid? MaintenancePlanId { get; set; }

        public Status? Status { get; set; }
    }
}
