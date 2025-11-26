using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class ModelMapper : Profile
    {
        public ModelMapper()
        {
            CreateMap<ModelRequest, Model>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Code, opt => opt.Ignore())
                .ForMember(d => d.Status, opt => opt.Ignore());

            CreateMap<ModelUpdateRequest, Model>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Code, opt => opt.Ignore());

            CreateMap<Model, ModelResponse>()
                .ForMember(d => d.MaintenancePlan, opt => opt.MapFrom(s => s.MaintenancePlan))
                .ForMember(d => d.Vehicles, opt => opt.MapFrom(s => s.Vehicles));
            //.ForMember(d => d.ModelPartTypes, opt => opt.MapFrom(s => s.ModelPartTypes));
        }
    }
}
