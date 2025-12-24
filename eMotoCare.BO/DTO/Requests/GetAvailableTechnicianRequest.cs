

using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class GetAvailableTechnicianRequest
    {
        public SlotTime SlotTime { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
