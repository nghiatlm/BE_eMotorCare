using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMotoCare.BO.DTO.Requests
{
    public class PayOSCreatePaymentRequest
    {
        [Required]
        public double Amount { get; set; }
        public string? Note { get; set; }
        public string? ReturnUrl { get; set; }
        public string? CancelUrl { get; set; }
    }
}
