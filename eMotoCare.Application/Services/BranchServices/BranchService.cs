using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using eMotoCare.Common.Enums;
using eMotoCare.Common.Exceptions;
using eMotoCare.Common.Models.Pages;
using eMotoCare.Common.Models.Requests;
using eMotoCare.Common.Models.Responses;
using eMotoCare.DAL;
using eMotoCare.DAL.Entities;
using Microsoft.Extensions.Logging;

namespace eMotoCare.BLL.Services.BranchServices
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BranchService> _logger;

        public BranchService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BranchService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<BranchResponse>> GetPagedAsync(
            string? search,
            Status? status,
            int page,
            int pageSize,
            CancellationToken ct = default
        )
        {
            var (items, total) = await _unitOfWork.Branches.GetPagedAsync(
                search,
                status,
                page,
                pageSize,
                ct
            );
            var data = _mapper.Map<List<BranchResponse>>(items);
            return new PageResult<BranchResponse>(data, pageSize, page, (int)total);
        }

        public async Task<BranchResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Branches.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<BranchResponse>(entity);
        }

        public async Task<Guid> CreateAsync(BranchRequest req, CancellationToken ct = default)
        {
            if (
                string.IsNullOrWhiteSpace(req.BranchName)
                || string.IsNullOrWhiteSpace(req.Address)
                || string.IsNullOrWhiteSpace(req.PhoneNumber)
                || string.IsNullOrWhiteSpace(req.Email)
                || !req.ServiceCenterId.HasValue
                || !req.Status.HasValue
            )
            {
                throw new AppException(ErrorCode.NOT_NULL);
            }
            await EnsureUniqueAsync(req, null, ct);

            var entity = _mapper.Map<Branch>(req);
            entity.ServiceCenterId = req.ServiceCenterId!.Value;
            entity.Status = req.Status!.Value;

            await _unitOfWork.Branches.CreateAsync(entity);
            await _unitOfWork.SaveChangesWithTransactionAsync();
            return entity.BranchId;
        }

        public async Task UpdateAsync(Guid id, BranchRequest req, CancellationToken ct = default)
        {
            var entity =
                await _unitOfWork.Branches.GetByIdAsync(id)
                ?? throw new AppException(ErrorCode.NOT_FOUND);

            await EnsureUniqueAsync(req, id, ct);
            if (req.BranchName != null)
                entity.BranchName = req.BranchName;
            if (req.Address != null)
                entity.Address = req.Address;
            if (req.PhoneNumber != null)
                entity.PhoneNumber = req.PhoneNumber;
            if (req.Email != null)
                entity.Email = req.Email;
            if (req.ManageById.HasValue)
                entity.ManageById = req.ManageById;
            if (req.ServiceCenterId.HasValue)
                entity.ServiceCenterId = req.ServiceCenterId.Value;
            if (req.Status.HasValue)
                entity.Status = req.Status.Value;

            await _unitOfWork.Branches.UpdateAsync(entity);
            await _unitOfWork.SaveChangesWithTransactionAsync();
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity =
                await _unitOfWork.Branches.GetByIdAsync(id)
                ?? throw new AppException(ErrorCode.NOT_FOUND);
            await _unitOfWork.Branches.DeleteAsync(entity);
            await _unitOfWork.SaveChangesWithTransactionAsync();
        }

        private async Task EnsureUniqueAsync(
            BranchRequest req,
            Guid? exceptId,
            CancellationToken ct
        )
        {
            if (
                req.BranchName != null
                && await _unitOfWork.Branches.ExistsNameAsync(req.BranchName, exceptId, ct)
            )
                throw new AppException(ErrorCode.HAS_EXISTED);

            if (
                req.PhoneNumber != null
                && await _unitOfWork.Branches.ExistsPhoneAsync(req.PhoneNumber, exceptId, ct)
            )
                throw new AppException(ErrorCode.HAS_EXISTED);

            if (
                req.Email != null
                && await _unitOfWork.Branches.ExistsEmailAsync(req.Email, exceptId, ct)
            )
                throw new AppException(ErrorCode.HAS_EXISTED);

            if (
                req.Address != null
                && await _unitOfWork.Branches.ExistsAddressAsync(req.Address, exceptId, ct)
            )
                throw new AppException(ErrorCode.HAS_EXISTED);
        }
    }
}
