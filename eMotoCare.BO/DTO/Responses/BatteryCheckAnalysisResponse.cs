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
        public int[] Time { get; set; } = Array.Empty<int>();
        public float[] Voltage { get; set; } = Array.Empty<float>();
        public float[] Current { get; set; } = Array.Empty<float>();
        public float[] Power { get; set; } = Array.Empty<float>();
        public float[] Capacity { get; set; } = Array.Empty<float>();
        public float[] Energy { get; set; } = Array.Empty<float>();
        public float[] Temp { get; set; } = Array.Empty<float>();
        public float[] SOC { get; set; } = Array.Empty<float>();
        public float[] SOH { get; set; } = Array.Empty<float>();
        public int SampleCount { get; set; }
        public BatteryConclusionResponse? Conclusion { get; set; }
        public VehicleResponse? Vehicle { get; set; }
    }
}
