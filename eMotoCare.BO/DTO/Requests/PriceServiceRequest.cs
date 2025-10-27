using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Requests
{
    public class PriceServiceRequest
    {
        [Required]
        public Guid PartTypeId { get; set; }

        [Required, StringLength(50)]
        public string Code { get; set; } = default!;

        [Required]
        public Remedies Remedies { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; } = default!;

        [Required]
        public decimal LaborCost { get; set; }

        [Required]
        public DateTime EffectiveDate { get; set; }

        [Required]
        public decimal Price { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }
    }
}
