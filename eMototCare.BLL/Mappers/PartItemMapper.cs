
using AutoMapper;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class PartItemMapper : Profile
    {
        public PartItemMapper()
        {
            CreateMap<PartItem, PartItemResponse>();
        }
    }
}
