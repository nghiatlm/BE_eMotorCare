using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class VehiclePartItemMapper : Profile
    {
        public VehiclePartItemMapper()
        {
            CreateMap<VehiclePartItemRequest, VehiclePartItem>()
                .ForMember(d => d.Id, opt => opt.Ignore());

            CreateMap<VehiclePartItem, VehiclePartItemResponse>();
        }
    }
}
