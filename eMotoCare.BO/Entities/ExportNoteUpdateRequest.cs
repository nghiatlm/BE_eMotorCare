﻿

using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    public class ExportNoteUpdateRequest
    {
        [Required]
        public string Code { get; set; } = string.Empty;

        [Required]
        public DateTime ExportDate { get; set; }

        [Required]
        [EnumDataType(typeof(ExportType))]
        public ExportType Type { get; set; }

        public string? ExportTo { get; set; }

        [Required]
        public int TotalQuantity { get; set; }

        [Required]
        public decimal TotalValue { get; set; }

        public string? Note { get; set; }

        [Required]
        public Guid ExportById { get; set; }
      
        [Required]
        public Guid ServiceCenterId { get; set; }

        [EnumDataType(typeof(ExportNoteStatus))]
        public ExportNoteStatus ExportNoteStatus { get; set; }

    }
}
