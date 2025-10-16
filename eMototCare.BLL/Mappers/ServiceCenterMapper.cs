using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class ServiceCenterMapper : Profile
    {
        public ServiceCenterMapper()
        {
            CreateMap<ServiceCenterRequest, ServiceCenter>()
                .ForMember(d => d.Id, opt => opt.Ignore());

            CreateMap<ServiceCenter, ServiceCenterResponse>();
        }
    }
}
