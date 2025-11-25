using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Responses
{
    public class ModelPartTypeResponse
    {
        public Guid Id { get; set; }
        public Guid ModelId { get; set; }
        public string? ModelName { get; set; }

        public Guid PartTypeId { get; set; }
        public string? PartTypeName { get; set; }

        public Status Status { get; set; }
    }
}
