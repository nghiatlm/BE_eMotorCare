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
        [Column("program_code", TypeName = "varchar(50)")]
        public string Code { get; set; }
        [Required]
        [Column("name", TypeName = "varchar(100)")]
        public string Name { get; set; }
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
        [Required]
        [Column("program_type", TypeName = "varchar(20)")]
        [EnumDataType(typeof(ProgramType))]
        public ProgramType ProgramType { get; set; }
        [Required]
        [Column("severity_level", TypeName = "varchar(20)")]
        [EnumDataType(typeof(SeverityLevel))]
        public SeverityLevel SeverityLevel { get; set; }
        [Required]
        [Column("created_by")]
        public Guid CreatedBy { get; set; }
        [Column("updated_by")]
        public Guid? UpdatedBy { get; set; }
        public virtual ICollection<ProgramDetail>? ProgramDetails { get; set; }
    }
}