

using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class BatteryCheck
    {
        public Guid Id { get; set; }
        public Guid EVCheckDetailId{ get; set; }
        public string BatteryLevel { get; set; }
        public string Voltage { get; set; }
        public Status Status { get; set; }
        public DateTime CheckDate { get; set; }
        public string? Note { get; set; }
    }
}