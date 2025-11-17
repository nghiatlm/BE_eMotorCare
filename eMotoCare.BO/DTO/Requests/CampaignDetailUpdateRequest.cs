

using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Requests
{
    public class CampaignDetailUpdateRequest
    {


        public Guid? CampaignId { get; set; }


        public string? Description { get; set; }


        public Guid? PartId { get; set; }



        public CampaignActionType? ActionType { get; set; }


        public string? Note { get; set; }

 
        public bool? IsMandatory { get; set; }

       
        public int? EstimatedTime { get; set; }
    }
}
