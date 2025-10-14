namespace eMotoCare.Common.Models.Responses
{
    public class ServiceCenterResponse
    {
        public Guid Id { get; set; }
        public string CenterName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
