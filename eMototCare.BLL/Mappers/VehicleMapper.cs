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
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.VinNUmber, opt => opt.MapFrom(s => s.VinNumber));

            CreateMap<Vehicle, VehicleResponse>()
                .ForMember(d => d.VinNumber, opt => opt.MapFrom(s => s.VinNUmber));
        }
    }
}
