

using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Requests
{
    public class RMARequest
    {

        [Required]
        public string ReturnAddress { get; set; } = string.Empty;

        public string? Note { get; set; }

        [Required]
        public Guid CreateById { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
    }
}
