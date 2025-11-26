using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Responses
{
    public class ModelPartResponse
    {
        public Guid Id { get; set; }
        public Guid ModelId { get; set; }
        public string? ModelName { get; set; }

        public Guid PartId { get; set; }
        public string? PartName { get; set; }

    }
}
