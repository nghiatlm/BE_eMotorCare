

using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class BatteryCheck
    {
        public Guid BatteryCheckId { get; set; }
        public Guid EVCheckDetailId{ get; set; }
        public EVCheckDetail? EVCheckDetail { get; set; }
        public string BatteryLevel { get; set; }
        public string Voltage { get; set; }
        public Status Status { get; set; }
        public DateTime CheckDate { get; set; }
        public string? Note { get; set; }
    }
}