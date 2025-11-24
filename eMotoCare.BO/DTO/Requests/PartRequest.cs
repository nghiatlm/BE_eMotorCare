
using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class PartRequest
    {
        [Required]
        public Guid PartTypeId { get; set; }
        public Guid? PartId { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Image { get; set; }

        public List<PartItemRequest>? PartItemRequest { get; set; }
    }
}
