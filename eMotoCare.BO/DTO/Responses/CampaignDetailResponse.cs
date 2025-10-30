
using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Responses
{
    public class CampaignDetailResponse
    {
        public Guid Id { get; set; }
        public CampaignResponse? Campaign { get; set; }
        public string Description { get; set; }
        public PartResponse? Part { get; set; }
        public CampaignActionType ActionType { get; set; }
        public string? Note { get; set; }
        public bool IsMandatory { get; set; }
        public int? EstimatedTime { get; set; }

    }
}
