
using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class ProgramRequest
    {
        [Required(ErrorMessage = "Program type is required.")]
        [EnumDataType(typeof(ProgramType))]
        public ProgramType Type { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? AttachmentUrl { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public Guid CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public List<VehicleModel>? VehicleModels { get; set; }

        public List<ProgramDetailRequest>? ProgramDetails { get; set; }
    }
}