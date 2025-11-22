using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eMotoCare.BO.DTO.Responses;

namespace eMototCare.BLL.Services.BatteryCheckServices
{
    public interface IBatteryCheckService
    {
        Task<BatteryCheckAnalysisResponse> ImportFromCsvAsync(
            Guid evCheckDetailId,
            Stream fileStream,
            CancellationToken ct = default
        );
    }
}
