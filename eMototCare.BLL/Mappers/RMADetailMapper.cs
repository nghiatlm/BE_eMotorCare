

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class RMADetailMapper : Profile
    {
        public RMADetailMapper() 
        { 
            CreateMap<RMADetail, RMADetailResponse>();
            CreateMap<RMADetailRequest, RMADetail>();
            CreateMap<RMADetailUpdateRequest, RMADetail>();
        }
    }
}
