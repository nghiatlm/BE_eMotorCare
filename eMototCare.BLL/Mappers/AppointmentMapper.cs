using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class AppointmentMapper : Profile
    {
        public AppointmentMapper()
        {
            CreateMap<AppointmentRequest, Appointment>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Code, opt => opt.Ignore())
                .ForMember(d => d.ApproveById, opt => opt.Ignore())
                .ForMember(d => d.EVCheck, opt => opt.Ignore())
                .ForMember(d => d.Payments, opt => opt.Ignore());

            CreateMap<Appointment, AppointmentResponse>()
                .ForMember(d => d.CheckinCode, opt => opt.MapFrom(s => s.Code));
        }
    }
}
