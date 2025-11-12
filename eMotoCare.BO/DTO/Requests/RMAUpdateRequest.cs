

using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Requests
{
    public class RMAUpdateRequest
    {



        public string? Code { get; set; }

        public DateTime? RMADate { get; set; }

        public string? ReturnAddress { get; set; }

        public RMAStatus? Status { get; set; }

        public string? Note { get; set; }


        public Guid? CreateById { get; set; }


    }
}
