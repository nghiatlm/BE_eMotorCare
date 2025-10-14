using eMotoCare.Common.Enums;

namespace eMotoCare.Common.Models.Requests
{
    public class StaffRequest
    {
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Address { get; set; }
        public string? CitizenId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public StaffPosition? StaffPosition { get; set; }
        public Guid? BranchId { get; set; }
        public string? StaffCode { get; set; }
        public string? Avatar { get; set; }
    }
}
