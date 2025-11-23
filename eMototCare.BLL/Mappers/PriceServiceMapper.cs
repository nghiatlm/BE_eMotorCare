using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class PriceServiceMapper : Profile
    {
        public PriceServiceMapper()
        {
            CreateMap<PriceServiceRequest, PriceService>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Code, opt => opt.Ignore());

            CreateMap<PriceService, PriceServiceResponse>()
                .ForMember(
                    d => d.PartTypeName,
                    opt => opt.MapFrom(s => s.PartType != null ? s.PartType.Name : null)
                );
        }
    }
}
