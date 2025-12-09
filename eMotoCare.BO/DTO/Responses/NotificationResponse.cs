

using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Responses
{
    public class NotificationResponse
    {

        public Guid Id { get; set; }

        public Account? Receiver { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public NotificationEnum Type { get; set; }

        public DateTime SentAt { get; set; }

        public bool IsRead { get; set; }
    }
}
