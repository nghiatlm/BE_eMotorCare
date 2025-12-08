

using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Requests
{
    public class NotificationRequest
    {
        [Required]
        public Guid ReceiverId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public NotificationEnum Type { get; set; }
        [Required]
        public DateTime SentAt { get; set; }

    }
}
