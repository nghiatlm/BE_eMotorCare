

using AutoMapper;

namespace eMototCare.BLL.Mappers
{
    public class ImportNoteMapper : Profile
    {
        public ImportNoteMapper()
        {
            CreateMap<eMotoCare.BO.Entities.ImportNote, eMotoCare.BO.DTO.Responses.ImportNoteResponse>();
        }
    }
}
