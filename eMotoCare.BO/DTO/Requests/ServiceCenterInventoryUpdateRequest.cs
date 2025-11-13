

using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Requests
{
    public class ServiceCenterInventoryUpdateRequest
    {

        public Guid? ServiceCenterId { get; set; }

        public string? ServiceCenterInventoryName { get; set; }

        public Status? Status { get; set; }
    }
}
