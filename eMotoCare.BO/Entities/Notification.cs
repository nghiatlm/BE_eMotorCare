

using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("notification")]
    public class Notification
    {
        [Key]
        [Column("notification_id")]
        public Guid Id { get; set; }
        [Required]
        [Column("receiver_id")]
        public Guid ReceiverId { get; set; }
        [ForeignKey(nameof(ReceiverId))]
        public Account? Receiver { get; set; }
        [Required]
        [Column("title", TypeName = "nvarchar(200)")]
        public string Title { get; set; }
        [Required]
        [Column("message", TypeName = "nvarchar(400)")]
        public string Message { get; set; }
        [Required]
        [Column("type", TypeName = "varchar(200)")]
        public NotificationEnum Type {  get; set; }
        [Required]
        [Column("send_at")]
        public DateTime SentAt { get; set; }
        [Required]
        [Column("is_read")]
        public bool IsRead { get; set; }

    }
}
