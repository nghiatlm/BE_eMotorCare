
using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Responses
{
    public class ProgramResponse
    {
        public Guid Id { get; set; }

        [EnumDataType(typeof(ProgramType))]
        public ProgramType Type { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }

        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }

        public string? AttachmentUrl { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

    }
}