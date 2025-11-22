using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class CampaignRequest
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Required]
        [EnumDataType(typeof(CampaignType))]
        public CampaignType Type { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
        public string? ModelName { get; set; }
    }
}
