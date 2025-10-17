
using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Responses
{
    public class PartMaintenanceStageDetailResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
    }
}
