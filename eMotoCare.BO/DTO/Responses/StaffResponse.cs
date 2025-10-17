using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Responses
{
    public class StaffResponse
    {
        public Guid Id { get; set; }
        public string StaffCode { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string CitizenId { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public GenderEnum? Gender { get; set; }
        public string? AvatarUrl { get; set; }
        public PositionEnum Position { get; set; }

        public Guid AccountId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
