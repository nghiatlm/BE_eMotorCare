

using AutoMapper;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class CampaignMapper : Profile
    {
        public CampaignMapper()
        {
            CreateMap<Campaign, CampaignResponse>();
        }
    }
}
