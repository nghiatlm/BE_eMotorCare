
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Requests
{
    public class CampaignDetailRequest
    {


        [Required]
        public Guid CampaignId { get; set; }

        public string Description { get; set; }

        [Required]
        public Guid PartId { get; set; }


        [Required]
        [EnumDataType(typeof(CampaignActionType))]
        public CampaignActionType ActionType { get; set; }

        public string? Note { get; set; }

        [Required]
        public bool IsMandatory { get; set; }

        public int? EstimatedTime { get; set; }
    }
}
