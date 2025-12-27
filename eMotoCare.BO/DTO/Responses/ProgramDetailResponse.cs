using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Responses
{
    public class ProgramDetailResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }
        [EnumDataType(typeof(ProgramType))]
        public ProgramType ProgramType { get; set; }
        [EnumDataType(typeof(SeverityLevel))]
        public SeverityLevel SeverityLevel { get; set; }
        public Guid CreatedBy { get; set; }        
        public Guid? UpdatedBy { get; set; }
        public virtual ICollection<ProgramDetail>? ProgramDetails { get; set; }
    }
}