

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class PartMapper : Profile
    {
        public PartMapper()
        {
            CreateMap<Part, PartMaintenanceStageDetailResponse>();
            CreateMap<Part, PartResponse>();
            CreateMap<PartRequest, Part>();
        }
    }
}
