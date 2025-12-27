
using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class ProgramRequest
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [EnumDataType(typeof(ProgramType))]
        public ProgramType ProgramType { get; set; }

        [EnumDataType(typeof(SeverityLevel))]
        public SeverityLevel SeverityLevel { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public ProgramDetailRequest ProgramDetailRequest { get; set; }
    }
}