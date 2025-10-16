using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Enum;

namespace eMotoCare.BO.Entities
{
    [Table("campaign")]
    public class Campaign
    {
        [Key]
        [Column("campaign_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("code")]
        public string Code { get; set; } = default!;

        [Required]
        [Column("name")]
        public string Name { get; set; } = default!;

        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("type")]
        public CampaignType Type { get; set; }

        [Required]
        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Column("status")]
        public Status Status { get; set; }
        public virtual ICollection<CampaignDetail>? CampaignDetails { get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; }
    }
}
