

using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Responses
{
    public class CustomerResponse
    {
        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Address { get; set; }

        [Required]
        public string CitizenId { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public GenderEnum? Gender { get; set; }

        public string? AvatarUrl { get; set; }

        public AccountResponse Account { get; set; }
    }
}
