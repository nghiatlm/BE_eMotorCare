using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMotoCare.BO.DTO.Responses
{
    public class BatteryConclusionResponse
    {
        public string energyCapability { get; set; } = string.Empty;
        public string chargeDischargeEfficiency { get; set; } = string.Empty;
        public string degradationStatus { get; set; } = string.Empty;
        public string remainingUsefulLife { get; set; } = string.Empty;
        public string safety { get; set; } = string.Empty;
        public string solution { get; set; } = string.Empty;
    }
}
