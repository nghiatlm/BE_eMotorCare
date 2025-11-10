

using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Responses
{
    public class RMAResponse
    {

        public Guid Id { get; set; }


        public string Code { get; set; }


        public DateTime RMADate { get; set; }


        public string ReturnAddress { get; set; }


        public RMAStatus Status { get; set; }

        public string? Note { get; set; }


        public CustomerResponse? Customer { get; set; }

        public StaffResponse? Staff { get; set; }

        public ICollection<RMADetailResponse>? RMADetails { get; set; }
    }
}
