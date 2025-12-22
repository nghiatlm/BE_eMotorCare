

using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Responses
{
    public class SyncCustomerResponse
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CitizenId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public GenderEnum? Gender { get; set; }
        public VehicleResponse? VehicleResponse { get; set; }
    }
        
}
