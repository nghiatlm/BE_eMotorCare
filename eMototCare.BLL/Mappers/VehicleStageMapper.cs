using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class VehicleStageMapper : Profile
    {
        public VehicleStageMapper()
        {
            CreateMap<VehicleStageRequest, VehicleStage>()
                .ForMember(d => d.Id, opt => opt.Ignore());

            CreateMap<VehicleStage, VehicleStageResponse>()
                .ForMember(d => d.MaintenanceStage, o => o.MapFrom(s => s.MaintenanceStage));
        }
    }
}
