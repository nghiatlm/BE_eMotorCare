using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Responses
{
    public class EVCheckDetailItemResponse
    {
        public Guid Id { get; set; }
        public Guid PartItemId { get; set; }
        public Guid? ReplacePartId { get; set; }
        public string? Result { get; set; }
        public Remedies Remedies { get; set; }
        public string? Unit { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? PricePart { get; set; }
        public decimal? PriceService { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
