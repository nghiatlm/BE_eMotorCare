
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("campaign")]
    public class Campaign
    {
        [Key]
        [Column("campaign_id")]
        public Guid Id { get; set; }
        public virtual ICollection<CampaignDetail>? CampaignDetails { get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; }
    }
}