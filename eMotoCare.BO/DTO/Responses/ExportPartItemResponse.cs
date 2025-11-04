

using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Responses
{
    public class ExportPartItemResponse
    {
        public Guid Id { get; set; }
        public PartResponse? Part { get; set; }
        public int Quantity { get; set; }
        public string SerialNumber { get; set; }
        public decimal Price { get; set; }
        public PartItemStatus Status { get; set; }
        public int? WarrantyPeriod { get; set; }
        public DateTime? WarantyStartDate { get; set; }
        public DateTime? WarantyEndDate { get; set; }
    }
}
