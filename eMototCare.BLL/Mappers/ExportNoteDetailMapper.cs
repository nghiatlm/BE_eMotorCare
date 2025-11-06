

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class ExportNoteDetailMapper : Profile
    {
        public ExportNoteDetailMapper()
        {
            CreateMap<ExportNoteDetail, ExportNoteDetailResponse>();
            CreateMap<ExportNoteDetailRequest, ExportNoteDetail>();
        }
    }
}
