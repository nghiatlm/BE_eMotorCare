

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class ExportNoteMapper : Profile
    {
        public ExportNoteMapper()
        {
            CreateMap<ExportNote, ExportNoteResponse>();
            CreateMap<ExportNoteRequest, ExportNote>();
            CreateMap<ExportNoteUpdateRequest, ExportNote>();
        }
    }
}
