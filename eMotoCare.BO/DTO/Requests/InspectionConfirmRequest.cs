using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class InspectionConfirmRequest
    {
        [Required]
        public bool Accept { get; set; }
    }
}
