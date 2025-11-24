

using AutoMapper;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class ProgramMapper : Profile
    {
        public ProgramMapper()
        {
            CreateMap<Program, ProgramDetailResponse>().ReverseMap();
            CreateMap<Program, ProgramResponse>().ReverseMap();
        }
    }
}