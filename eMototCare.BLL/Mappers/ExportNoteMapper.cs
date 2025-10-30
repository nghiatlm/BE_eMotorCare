

using AutoMapper;

namespace eMototCare.BLL.Mappers
{
    public class ExportNoteMapper : Profile
    {
        public ExportNoteMapper()
        {
            CreateMap<eMotoCare.BO.Entities.ExportNote, eMotoCare.BO.DTO.Responses.ExportNoteResponse>();
        }
    }
}
