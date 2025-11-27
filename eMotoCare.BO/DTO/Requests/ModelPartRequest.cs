using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class ModelPartRequest
    {
        public Guid ModelId { get; set; }
        public Guid PartId { get; set; }
    }
}
