
using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Responses
{
    public class ProgramResponse
    {
        public Guid Id { get; set; }

        [EnumDataType(typeof(ProgramType))]
        public ProgramType ProgramType { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }

        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

    }
}