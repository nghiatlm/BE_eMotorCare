using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class ModelPartRequest
    {
        [Required]
        public Guid ModelId { get; set; }

        [Required]
        public Guid PartId { get; set; }
    }
}
