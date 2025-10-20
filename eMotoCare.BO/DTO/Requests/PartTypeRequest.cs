
using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class PartTypeRequest
    {
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string? Description { get; set; }
    }
}
