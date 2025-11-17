

using AutoMapper;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.DTO.Requests;
namespace eMototCare.BLL.Mappers
{
    public class CampaignDetailMapper : Profile
    {
        public CampaignDetailMapper()
        {
            CreateMap<CampaignDetail, CampaignDetailResponse>();
            CreateMap<CampaignDetailRequest, CampaignDetail>();
            CreateMap<CampaignDetailUpdateRequest, CampaignDetail>();
        }
    }
}
