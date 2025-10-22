using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class CheckInRequest
    {
        [Required]
        public string Code { get; set; } = default!;
    }
}
