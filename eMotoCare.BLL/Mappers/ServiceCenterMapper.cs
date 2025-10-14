using AutoMapper;
using eMotoCare.Common.Models.Requests;
using eMotoCare.Common.Models.Responses;
using eMotoCare.DAL.Entities;

namespace eMotoCare.BLL.Mappers
{
    public class ServiceCenterMapper : Profile
    {
        public ServiceCenterMapper()
        {
            CreateMap<ServiceCenterRequest, ServiceCenter>()
                .ForMember(d => d.ServiceCenterId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore());

            CreateMap<ServiceCenter, ServiceCenterResponse>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.ServiceCenterId));
        }
    }
}
