using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMotoCare.BO.DTO.Responses
{
    public class BatteryCheckAnalysisResponse
    {
        public Guid Id { get; set; }
        public Guid EVCheckDetailId { get; set; }
        public int SampleCount { get; set; }
        public float MinVoltage { get; set; }
        public float MaxVoltage { get; set; }
        public float AvgVoltage { get; set; }
        public float MinCurrent { get; set; }
        public float MaxCurrent { get; set; }
        public float AvgCurrent { get; set; }
        public float MinTemp { get; set; }
        public float MaxTemp { get; set; }
        public float AvgTemp { get; set; }
        public float MinSOC { get; set; }
        public float MaxSOC { get; set; }
        public float AvgSOC { get; set; }
        public float MinSOH { get; set; }
        public float MaxSOH { get; set; }
        public float AvgSOH { get; set; }
        public BatteryConclusionResponse? Conclusion { get; set; }
        public VehicleResponse? Vehicle { get; set; }
    }
}
