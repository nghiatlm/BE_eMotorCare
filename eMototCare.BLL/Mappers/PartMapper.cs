

using AutoMapper;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class PartMapper : Profile
    {
        public PartMapper()
        {
            CreateMap<Part, PartMaintenanceStageDetailResponse>();
        }
    }
}
