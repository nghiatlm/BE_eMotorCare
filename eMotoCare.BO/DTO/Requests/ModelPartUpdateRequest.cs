using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Requests
{
    public class ModelPartUpdateRequest
    {
        public Guid? ModelId { get; set; }
        public Guid? PartId { get; set; }
    }
}
