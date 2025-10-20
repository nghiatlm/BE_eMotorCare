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
            CreateMap<EVCheckUpsertRequest, EVCheck>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.AppointmentId, opt => opt.Ignore())
                .ForMember(d => d.TaskExecutorId, opt => opt.Ignore())
                .ForMember(d => d.EVCheckDetails, opt => opt.Ignore());

            CreateMap<EVCheckDetailItemRequest, EVCheckDetail>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.EVCheckId, opt => opt.Ignore())
                .ForMember(d => d.Status, opt => opt.Ignore());

            CreateMap<EVCheckDetail, EVCheckDetailItemResponse>();
            CreateMap<EVCheck, EVCheckResponse>()
                .ForMember(d => d.Items, opt => opt.MapFrom(s => s.EVCheckDetails));
        }
    }
}
