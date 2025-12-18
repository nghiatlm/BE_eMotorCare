

namespace eMotoCare.BO.DTO.Requests
{
    public class CustomerSyncRequest
    {
        public string CitizenId { get; set; } = string.Empty;
        public Guid AccountId { get; set; }
        public string ChassisNumber { get; set; }
    }
}