

using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class PartItemUpdateRequest
    {
        public Guid? PartId { get; set; }
        public Guid? ExportNoteId { get; set; }
        public Guid? ImportNoteId { get; set; }
        public int? Quantity { get; set; }
        public string? SerialNumber { get; set; }
        public decimal? Price { get; set; }
        [EnumDataType(typeof(PartItemStatus))]
        public PartItemStatus? Status { get; set; }
        public int? WarrantyPeriod { get; set; }
        public DateTime? WarantyStartDate { get; set; }
        public DateTime? WarantyEndDate { get; set; }
        public Guid? ServiceCenterInventoryId { get; set; }
    }
}
