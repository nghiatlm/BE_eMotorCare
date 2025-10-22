

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class EVCheckMapper : Profile
    {
        public EVCheckMapper()
        {
            CreateMap<EVCheckRequest, EVCheck>();
            CreateMap<EVCheckUpdateRequest, EVCheck>();
            CreateMap<EVCheck, EVCheckResponse>();
        }
    }
}
