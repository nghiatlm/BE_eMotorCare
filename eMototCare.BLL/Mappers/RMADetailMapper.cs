

using AutoMapper;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class RMADetailMapper : Profile
    {
        public RMADetailMapper() 
        { 
            CreateMap<RMADetail, RMADetailResponse>();
        }
    }
}
