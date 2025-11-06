using eMotoCare.BO.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Requests
{
    public class ExportNoteDetailRequest
    {
        [Required]
        public Guid ExportNoteId { get; set; }
        [Required]
        public Guid PartItemId { get; set; }
        public string? Note { get; set; }
    }
}
