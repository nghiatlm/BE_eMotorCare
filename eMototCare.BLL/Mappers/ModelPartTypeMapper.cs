using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class ModelPartTypeMapper : Profile
    {
        public ModelPartTypeMapper()
        {
            CreateMap<ModelPartTypeRequest, ModelPartType>()
                .ForMember(d => d.Id, opt => opt.Ignore());

            CreateMap<ModelPartType, ModelPartTypeResponse>()
                .ForMember(
                    dest => dest.ModelName,
                    opt => opt.MapFrom(src => src.Model != null ? src.Model.Name : null)
                )
                .ForMember(
                    dest => dest.PartTypeName,
                    opt => opt.MapFrom(src => src.PartType != null ? src.PartType.Name : null)
                );
        }
    }
}
