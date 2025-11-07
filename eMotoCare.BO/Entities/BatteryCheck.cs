
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;

namespace eMotoCare.BO.Entities
{
    [Table("battery_check")]
    public class BatteryCheck : BaseEntity
    {
        [Key]
        [Column("battery_check_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("time", TypeName = "json")]
        public int[] Time { get; set; } = Array.Empty<int>();

        [Required]
        [Column("voltage", TypeName = "json")]
        public float[] Voltage { get; set; } = Array.Empty<float>();

        [Required]
        [Column("current", TypeName = "json")]
        public float[] current { get; set; } = Array.Empty<float>();


        [Required]
        [Column("power", TypeName = "json")]
        public float[] Power { get; set; } = Array.Empty<float>();

        [Required]
        [Column("capacity", TypeName = "json")]
        public float[] Capacity { get; set; } = Array.Empty<float>();

        [Required]
        [Column("energy", TypeName = "json")]
        public float[] Energy { get; set; } = Array.Empty<float>();

        [Required]
        [Column("temp", TypeName = "json")]
        public float[] Temp { get; set; } = Array.Empty<float>();

        [Required]
        [Column("soc", TypeName = "json")]
        public float[] SOC { get; set; } = Array.Empty<float>();

        [Required]
        [Column("soh", TypeName = "json")]
        public float[] SOH { get; set; } = Array.Empty<float>();

        [Column("solution", TypeName = "nvarchar(400)")]
        public string? Solution { get; set; }

        [Required]
        [Column("ev_check_detail_id")]
        public Guid EVCheckDetailId { get; set; }

        [ForeignKey(nameof(EVCheckDetailId))]
        public virtual EVCheckDetail? EVCheckDetail { get; set; }

    }
}