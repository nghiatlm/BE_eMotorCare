

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class ImportNoteMapper : Profile
    {
        public ImportNoteMapper()
        {
            CreateMap<ImportNote, ImportNoteResponse>();
            CreateMap<ImportNoteRequest, ImportNote>();
            CreateMap<ImportNoteUpdateRequest, ImportNote>();
        }
    }
}
