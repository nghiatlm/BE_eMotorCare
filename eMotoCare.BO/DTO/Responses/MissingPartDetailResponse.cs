namespace eMotoCare.BO.DTO.Responses
{
    public class MissingPartDetailResponse
    {
        public int Index { get; set; }
        public string? Image { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public int RequestedQty { get; set; }
        public string? SuggestCenter { get; set; }
        public string? StockStatus { get; set; }
    }
}
