
using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Requests
{
    public class ProgramDetailRequest
    {
        public Guid? PartId { get; set; }

        [EnumDataType(typeof(ActionType))]
        public ActionType? ActionType { get; set; }
        public string? Description { get; set; }
        public int? ManufactureYear { get; set; }
        public Guid? ModelId { get; set; }
    }
}