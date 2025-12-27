using AutoMapper;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Mappers
{
    public class ProgramMapper : Profile
    {
        public ProgramMapper()
        {
            CreateMap<Program, ProgramDetailResponse>().ReverseMap();
            CreateMap<Program, ProgramResponse>().ReverseMap();
            CreateMap<PageResult<Program>, PageResult<ProgramResponse>>();
        }
    }
}