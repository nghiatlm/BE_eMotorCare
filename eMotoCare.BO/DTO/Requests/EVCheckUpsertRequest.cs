using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class EVCheckUpsertRequest
    {
        [Required]
        public DateTime CheckDate { get; set; }

        [Required]
        public string Odometer { get; set; } = string.Empty;

        [Required]
        public EVCheckStatus Status { get; set; }
        public decimal? TotalAmout { get; set; }

        [Required, MinLength(1)]
        public List<EVCheckDetailItemRequest> Items { get; set; } = new();
    }
}
