using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class AssignTechnicianRequest
    {
        [Required]
        public Guid TechnicianId { get; set; }
    }
}
