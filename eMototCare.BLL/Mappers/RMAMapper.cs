

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class RMAMapper : Profile
    {
        public RMAMapper() 
        {
            CreateMap<RMARequest, RMA>();
            CreateMap<RMAUpdateRequest, RMA>();
            CreateMap<RMA, RMAResponse>();
        }
    }
}
