

using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Requests
{
    public class ImportNoteRequest
    {

        [Required]
        public string ImportFrom { get; set; } = string.Empty;
        [Required]
        public string? Supplier { get; set; }
        [Required]
        [Column("type", TypeName = "varchar(200)")]
        [EnumDataType(typeof(ImportType))]
        public ImportType Type { get; set; }
        [Required]
        public decimal? TotalAmout { get; set; }
        [Required]
        public Guid? ImportById { get; set; }
        [Required]
        public Guid ServiceCenterId { get; set; }

    }
}
