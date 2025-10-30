

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class MaintenanceStageMapper : Profile
    {
        public MaintenanceStageMapper()
        {
            CreateMap<MaintenanceStageRequest, MaintenanceStage>();
            CreateMap<MaintenanceStage, MaintenanceStageResponse>();
            CreateMap<MaintenanceStageUpdateRequest, MaintenanceStage>();
        }
    }
}
