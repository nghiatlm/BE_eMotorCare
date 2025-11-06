

using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    public class ExportNoteUpdateRequest
    {
        public string? Code { get; set; }

        public DateTime? ExportDate { get; set; }

        [EnumDataType(typeof(ExportType))]
        public ExportType? Type { get; set; }

        public string? ExportTo { get; set; }

        public int? TotalQuantity { get; set; }

        public decimal? TotalValue { get; set; }

        public string? Note { get; set; }

        public Guid ExportById { get; set; }
      
        public Guid? ServiceCenterId { get; set; }

        [EnumDataType(typeof(ExportNoteStatus))]
        public ExportNoteStatus? ExportNoteStatus { get; set; }

    }
}
