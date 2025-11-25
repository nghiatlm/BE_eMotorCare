
namespace eMotoCare.BO.DTO.Requests
{
    public class PartItemRequest
    {
        public int Quantity { get; set; }
        public string? SerialNumber { get; set; }
        public decimal Price { get; set; }
        public int? WarrantyPeriod { get; set; }
        public DateTime? WarantyStartDate { get; set; }
        public DateTime? WarantyEndDate { get; set; }
        public bool IsManufacturerWarranty { get; set; }
    }
}
