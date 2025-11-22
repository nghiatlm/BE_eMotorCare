using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Responses
{
    public class CampaignResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public CampaignType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Status Status { get; set; }
        public string? ModelName { get; set; }
        public ICollection<CampaignDetailResponse?> CampaignDetails { get; set; }
    }
}
