using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.BatteryCheckServices
{
    public interface IBatteryCheckService
    {
        Task<BatteryCheckAnalysisResponse> ImportFromCsvAsync(
            Guid evCheckDetailId,
            Stream fileStream,
            CancellationToken ct = default
        );
        Task<BatteryCheckAnalysisResponse> GetByIdAsync(Guid id);
        Task<PageResult<BatteryCheckAnalysisResponse>> GetPagedAsync(
            Guid? evCheckDetailId,
            Guid? vehicleId,
            DateTime? fromDate,
            DateTime? toDate,
            string? sortBy,
            bool sortDesc,
            int page,
            int pageSize
        );
    }
}
