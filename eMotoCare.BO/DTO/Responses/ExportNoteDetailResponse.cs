

using eMotoCare.BO.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Responses
{
    public class ExportNoteDetailResponse
    {
        public Guid Id { get; set; }
        public ExportNote? ExportNote { get; set; }
        public PartItem? PartItem { get; set; }
        public string? Note { get; set; }
    }
}
