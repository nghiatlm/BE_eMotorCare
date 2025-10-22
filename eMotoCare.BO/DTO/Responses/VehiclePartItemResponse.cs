using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMotoCare.BO.DTO.Responses
{
    public class VehiclePartItemResponse
    {
        public Guid Id { get; set; }
        public DateTime InstallDate { get; set; }
        public Guid VehicleId { get; set; }
        public Guid PartItemId { get; set; }
        public Guid? ReplaceForId { get; set; }
    }
}
