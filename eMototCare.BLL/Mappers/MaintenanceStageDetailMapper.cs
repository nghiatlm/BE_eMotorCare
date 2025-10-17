

using AutoMapper;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class MaintenanceStageDetailMapper : Profile
    {
        public MaintenanceStageDetailMapper()
        {
            CreateMap<MaintenanceStageDetail, MaintenanceStageDetailResponse>();
        }
    }
}
