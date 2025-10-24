using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class ServiceCenterSlotMapper : Profile
    {
        public ServiceCenterSlotMapper()
        {
            CreateMap<ServiceCenterSlotRequest, ServiceCenterSlot>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.ServiceCenterId, o => o.Ignore());

            CreateMap<ServiceCenterSlot, ServiceCenterSlotResponse>();
        }
    }
}
