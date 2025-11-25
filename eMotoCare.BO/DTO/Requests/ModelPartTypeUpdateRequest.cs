using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Requests
{
    public class ModelPartTypeUpdateRequest
    {
        public Guid? ModelId { get; set; }
        public Guid? PartTypeId { get; set; }
        public Status? Status { get; set; }
    }
}
