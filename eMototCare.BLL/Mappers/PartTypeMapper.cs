

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class PartTypeMapper : Profile
    {
        public PartTypeMapper()
        {
            CreateMap<PartTypeRequest, PartType>();
            CreateMap<PartType, PartTypeResponse>();
        }
    }
}
