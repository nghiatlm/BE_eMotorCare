

using eMotoCare.BO.Enum;
using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class PartTypeUpdateRequest
    {
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string? Description { get; set; }
        [Required]
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }
    }
}
