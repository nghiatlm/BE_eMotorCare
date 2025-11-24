
namespace eMotoCare.BO.DTO.Requests
{
    public class ProgramDetailRequest
    {
        public Guid? RecallPartId { get; set; }

        public string? ServiceType { get; set; }

        public int? DiscountPercent { get; set; }

        public int? BonusAmount { get; set; }

        public string? RecallAction { get; set; }
    }
}