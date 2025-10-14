

using System.ComponentModel.DataAnnotations;

namespace eMotoCare.Common.Models.Requests
{
    public class ForgetPasswordRequest
    {
        [Required]
        public string PhoneNumber { get; set; }
    }
}
