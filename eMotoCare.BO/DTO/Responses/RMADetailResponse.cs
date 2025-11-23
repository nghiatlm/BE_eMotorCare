

using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Responses
{
    public class RMADetailResponse
    {

        public Guid Id { get; set; }

        public int Quantity { get; set; }

        public string? Reason { get; set; }

        public string RMANumber { get; set; }

        public DateTime? ReleaseDateRMA { get; set; }

        public DateTime? ExpirationDateRMA { get; set; }

        public string? Inspector { get; set; }

        public string? Result { get; set; }

        public string? Solution { get; set; }

        public EVCheckDetailResponse? EVCheckDetail { get; set; }

        public RMAResponse? RMA { get; set; }

        public RMADetailStatus Status { get; set; }

        public PartItemResponse? ReplacePart { get; set; }
    }
}
