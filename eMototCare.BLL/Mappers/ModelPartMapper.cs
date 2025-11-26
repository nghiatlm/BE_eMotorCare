using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class ModelPartMapper : Profile
    {
        public ModelPartMapper()
        {
            CreateMap<ModelPartRequest, ModelPart>().ForMember(d => d.Id, opt => opt.Ignore());

            CreateMap<ModelPart, ModelPartResponse>()
                .ForMember(d => d.ModelName, opt => opt.MapFrom(src => src.Model!.Name))
                .ForMember(d => d.PartName, opt => opt.MapFrom(src => src.Part!.Name));
        }
    }
}
