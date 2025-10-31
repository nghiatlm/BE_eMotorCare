﻿using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.ExportNoteRepository
{
    public interface IExportNoteRepository : IGenericRepository<ExportNote>
    {
        Task<bool> ExistsCodeAsync(string code);
        Task<ExportNote?> GetByIdAsync(Guid id);
        Task<(IReadOnlyList<ExportNote> Items, long Total)> GetPagedAsync(string? code, DateTime? startDate, DateTime? endDate, ExportType? exportType, string? exportTo, int? totalQuantity, decimal? totalValue, Guid? exportById, Guid? serviceCenterId, ExportNoteStatus? exportNoteStatus, int page, int pageSize);
    }
}
