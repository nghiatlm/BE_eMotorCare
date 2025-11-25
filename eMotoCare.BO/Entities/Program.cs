
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.Entities
{
    [Table("program")]
    public class Program : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("program_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("name", TypeName = "varchar(100)")]
        [EnumDataType(typeof(ProgramType))]
        public ProgramType Type { get; set; }

        [Required]
        [Column("title", TypeName = "nvarchar(200)")]
        public string Title { get; set; } = string.Empty;

        [Column("description", TypeName = "longtext")]
        public string? Description { get; set; }

        [Required]
        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Required]
        [Column("status", TypeName = "varchar(20)")]
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }

        [Column("attachment_url", TypeName = "nvarchar(500)")]
        public string? AttachmentUrl { get; set; }

        [Column("created_by")]
        public Guid? CreatedBy { get; set; }
        [Column("updated_by")]
        public Guid? UpdatedBy { get; set; }

        public virtual ICollection<ProgramDetail>? ProgramDetails { get; set; }
        public virtual ICollection<ProgramModel>? ProgramModels { get; set; }
    }
}