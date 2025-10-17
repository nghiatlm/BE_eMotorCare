using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class CustomerRequest
    {
        [Required]
        public Guid AccountId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }

        [Required, StringLength(15)]
        public string CitizenId { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }
        public GenderEnum? Gender { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
