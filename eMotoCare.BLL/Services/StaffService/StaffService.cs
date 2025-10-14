using AutoMapper;
using eMotoCare.Common.Enums;
using eMotoCare.Common.Exceptions;
using eMotoCare.Common.Models.Pages;
using eMotoCare.Common.Models.Requests;
using eMotoCare.Common.Models.Responses;
using eMotoCare.DAL;
using eMotoCare.DAL.Entities;

namespace eMotoCare.BLL.Services.StaffService
{
    public class StaffService : IStaffService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public StaffService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<PageResult<StaffResponse>> GetPagedAsync(
            string? search,
            Gender? gender,
            StaffPosition? position,
            Guid? branchId,
            int page,
            int pageSize,
            CancellationToken ct = default
        )
        {
            var (items, total) = await _uow.Staffs.GetPagedAsync(
                search,
                gender,
                position,
                branchId,
                page,
                pageSize,
                ct
            );
            var data = _mapper.Map<List<StaffResponse>>(items);
            return new PageResult<StaffResponse>(data, pageSize, page, (int)total);
        }

        public async Task<StaffResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _uow.Staffs.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<StaffResponse>(entity);
        }

        public async Task<Guid> CreateAsync(StaffRequest req, CancellationToken ct = default)
        {
            if (
                string.IsNullOrWhiteSpace(req.FirstName)
                || string.IsNullOrWhiteSpace(req.LastName)
                || string.IsNullOrWhiteSpace(req.Address)
                || string.IsNullOrWhiteSpace(req.CitizenId)
                || string.IsNullOrWhiteSpace(req.StaffCode)
                || !req.DateOfBirth.HasValue
                || !req.Gender.HasValue
                || !req.StaffPosition.HasValue
                || !req.BranchId.HasValue
            )
                throw new AppException(ErrorCode.NOT_NULL);

            await EnsureUniqueAsync(req, null, ct);

            var branch = await _uow.Branches.GetByIdAsync(req.BranchId!.Value);
            if (branch is null)
                throw new AppException(ErrorCode.NOT_FOUND);

            var entity = _mapper.Map<Staff>(req);
            entity.BranchId = req.BranchId!.Value;
            entity.Gender = req.Gender!.Value;
            entity.StaffPosition = req.StaffPosition!.Value;
            entity.DateOfBirth = req.DateOfBirth!.Value;

            await _uow.Staffs.CreateAsync(entity);
            await _uow.SaveChangesWithTransactionAsync();
            var response = _mapper.Map<StaffResponse>(entity);
            return entity.StaffId;
        }

        public async Task UpdateAsync(Guid id, StaffRequest req, CancellationToken ct = default)
        {
            var entity =
                await _uow.Staffs.GetByIdAsync(id) ?? throw new AppException(ErrorCode.NOT_FOUND);

            await EnsureUniqueAsync(req, id, ct);

            if (req.FirstName != null)
                entity.FirstName = req.FirstName;
            if (req.LastName != null)
                entity.LastName = req.LastName;
            if (req.Address != null)
                entity.Address = req.Address;
            if (req.CitizenId != null)
                entity.CitizenId = req.CitizenId;
            if (req.StaffCode != null)
                entity.StaffCode = req.StaffCode;
            if (req.Avatar != null)
                entity.Avatar = req.Avatar;
            if (req.DateOfBirth.HasValue)
                entity.DateOfBirth = req.DateOfBirth.Value;
            if (req.Gender.HasValue)
                entity.Gender = req.Gender.Value;
            if (req.StaffPosition.HasValue)
                entity.StaffPosition = req.StaffPosition.Value;

            if (req.BranchId.HasValue)
            {
                var branch = await _uow.Branches.GetByIdAsync(req.BranchId.Value);
                if (branch is null)
                    throw new AppException(ErrorCode.NOT_FOUND);
                entity.BranchId = req.BranchId.Value;
            }

            await _uow.Staffs.UpdateAsync(entity);
            await _uow.SaveChangesWithTransactionAsync();
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity =
                await _uow.Staffs.GetByIdAsync(id) ?? throw new AppException(ErrorCode.NOT_FOUND);
            await _uow.Staffs.DeleteAsync(entity);
            await _uow.SaveChangesWithTransactionAsync();
        }

        private async Task EnsureUniqueAsync(StaffRequest req, Guid? exceptId, CancellationToken ct)
        {
            if (
                req.CitizenId != null
                && await _uow.Staffs.ExistsCitizenIdAsync(req.CitizenId, exceptId, ct)
            )
                throw new AppException(ErrorCode.HAS_EXISTED);

            if (
                req.StaffCode != null
                && await _uow.Staffs.ExistsStaffCodeAsync(req.StaffCode, exceptId, ct)
            )
                throw new AppException(ErrorCode.HAS_EXISTED);
        }
    }
}
