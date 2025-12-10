using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class VehicleMapper : Profile
    {
        public VehicleMapper()
        {
            CreateMap<VehicleRequest, Vehicle>()
                .ForMember(d => d.Id, opt => opt.Ignore());

            CreateMap<Vehicle, VehicleResponse>()
                .ForMember(
                    d => d.ModelName,
                    opt => opt.MapFrom(s => s.Model != null ? s.Model.Name : null)
                );
            CreateMap<VehicleUpdateRequest, Vehicle>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.ChassisNumber, opt => opt.Ignore())
                .ForMember(d => d.EngineNumber, opt => opt.Ignore())
                .ForMember(d => d.ManufactureDate, opt => opt.Ignore());
        }
    }
}
