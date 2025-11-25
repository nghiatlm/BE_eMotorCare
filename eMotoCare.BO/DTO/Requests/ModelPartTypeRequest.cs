using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class ModelPartTypeRequest
    {
        [Required]
        public Guid ModelId { get; set; }

        [Required]
        public Guid PartTypeId { get; set; }
    }
}
