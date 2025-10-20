using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Responses
{
    public class EVCheckResponse
    {
        public Guid Id { get; set; }
        public DateTime CheckDate { get; set; }
        public string Odometer { get; set; } = string.Empty;
        public EVCheckStatus Status { get; set; }
        public decimal? TotalAmout { get; set; }
        public List<EVCheckDetailItemResponse> Items { get; set; } = new();
    }
}
