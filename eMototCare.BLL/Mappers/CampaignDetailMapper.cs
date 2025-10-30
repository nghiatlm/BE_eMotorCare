

using AutoMapper;

namespace eMototCare.BLL.Mappers
{
    public class CampaignDetailMapper : Profile
    {
        public CampaignDetailMapper()
        {
            CreateMap<eMotoCare.BO.Entities.CampaignDetail, eMotoCare.BO.DTO.Responses.CampaignDetailResponse>();
        }
    }
}
