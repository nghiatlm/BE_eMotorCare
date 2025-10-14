using eMotoCare.Common.Enums;

namespace eMotoCare.Common.Models.Requests
{
    public class BranchRequest
    {
        public Guid? ManageById { get; set; }
        public Guid? ServiceCenterId { get; set; }

        public string? BranchName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public Status? Status { get; set; }
    }
}
