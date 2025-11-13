

using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Requests
{
    public class CampaignUpdateRequest
    {

        public string? Name { get; set; }
        public string? Code { get; set; }

        public string? Description { get; set; }

        [EnumDataType(typeof(CampaignType))]
        public CampaignType? Type { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [EnumDataType(typeof(CampaignStatus))]
        public CampaignStatus? Status { get; set; }

    }
}
