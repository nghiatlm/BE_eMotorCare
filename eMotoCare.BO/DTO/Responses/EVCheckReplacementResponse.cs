

namespace eMotoCare.BO.DTO.Responses
{
    public class EVCheckReplacementResponse
    {
        public Guid PartId { get; set; }
        public string PartName { get; set; }
        public string Code { get; set; }
        public int StockQuantity { get; set; }
        public bool Available => StockQuantity > 0;
    }
}
