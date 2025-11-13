

using AutoMapper;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class CampaignDetailMapper : Profile
    {
        public CampaignDetailMapper()
        {
            CreateMap<CampaignDetail, CampaignDetailResponse>();
        }
    }
}
