

using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class CustomerRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        [Required]
        public string CitizenId { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public GenderEnum? Gender { get; set; }
        public string? AvatarUrl { get; set; }
        public Guid AccountId { get; set; }
    }
}
