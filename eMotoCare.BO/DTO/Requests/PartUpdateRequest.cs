

using eMotoCare.BO.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Requests
{
    public class PartUpdateRequest
    {
        [Required]
        public Guid PartTypeId { get; set; }
        [Required]
        public string Code { get; set; } = default!;
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public int Quantity { get; set; }
        [Column("image")]
        public string? Image { get; set; }
        [Required]
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }
    }
}
