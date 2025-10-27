

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class EVCheckDetailMapper : Profile
    {
        public EVCheckDetailMapper()
        {
            CreateMap<EVCheckDetail, EVCheckDetailResponse>();
            CreateMap<EVCheckDetailRequest, EVCheckDetail>();
        }
    }
}
