

using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Requests
{
    public class RMADetailUpdateRequest
    {

        public int? Quantity { get; set; }

        public string? Reason { get; set; }

        public string? RMANumber { get; set; }

        public DateTime? ReleaseDateRMA { get; set; }

        public DateTime? ExpirationDateRMA { get; set; }

        public string? Inspector { get; set; }

        public string? Result { get; set; }

        public string? Solution { get; set; }

        public Guid? EVCheckDetailId { get; set; }

        public Guid? RMAId { get; set; }

        public RMADetailStatus? Status { get; set; }

        public PartItemRequest? ReplacePart { get; set; }
    }
}
