

using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Responses
{
    public class PartResponse
    {
        public Guid Id { get; set; }
        public PartTypeResponse? PartType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string? Image { get; set; }
        public Status Status { get; set; }
    }
}
