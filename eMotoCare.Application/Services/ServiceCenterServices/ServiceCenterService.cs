using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using eMotoCare.Common.Exceptions;
using eMotoCare.Common.Models.Pages;
using eMotoCare.Common.Models.Requests;
using eMotoCare.Common.Models.Responses;
using eMotoCare.DAL;
using eMotoCare.DAL.Entities;

namespace eMotoCare.BLL.Services.ServiceCenterServices
{
    public class ServiceCenterService : IServiceCenterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServiceCenterService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PageResult<ServiceCenterResponse>> GetPagedAsync(
            string? search,
            int page,
            int pageSize,
            CancellationToken ct = default
        )
        {
            var (items, total) = await _unitOfWork.ServiceCenters.GetPagedAsync(
                search,
                page,
                pageSize,
                ct
            );
            var data = _mapper.Map<List<ServiceCenterResponse>>(items);
            return new PageResult<ServiceCenterResponse>(data, pageSize, page, (int)total);
        }

        public async Task<ServiceCenterResponse?> GetByIdAsync(
            Guid id,
            CancellationToken ct = default
        )
        {
            var entity = await _unitOfWork.ServiceCenters.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<ServiceCenterResponse>(entity);
        }

        public async Task<Guid> CreateAsync(
            ServiceCenterRequest req,
            CancellationToken ct = default
        )
        {
            if (
                string.IsNullOrWhiteSpace(req.CenterName)
                || string.IsNullOrWhiteSpace(req.Address)
                || string.IsNullOrWhiteSpace(req.PhoneNumber)
                || string.IsNullOrWhiteSpace(req.Email)
            )
                throw new AppException(ErrorCode.NOT_NULL);

            await EnsureUniqueAsync(req, null, ct);

            var entity = _mapper.Map<ServiceCenter>(req);
            await _unitOfWork.ServiceCenters.CreateAsync(entity);
            await _unitOfWork.SaveChangesWithTransactionAsync();

            return entity.ServiceCenterId;
        }

        public async Task UpdateAsync(
            Guid id,
            ServiceCenterRequest req,
            CancellationToken ct = default
        )
        {
            var entity =
                await _unitOfWork.ServiceCenters.GetByIdAsync(id)
                ?? throw new AppException(ErrorCode.NOT_FOUND);

            await EnsureUniqueAsync(req, id, ct);

            if (req.CenterName != null)
                entity.CenterName = req.CenterName;
            if (req.Address != null)
                entity.Address = req.Address;
            if (req.PhoneNumber != null)
                entity.PhoneNumber = req.PhoneNumber;
            if (req.Email != null)
                entity.Email = req.Email;

            await _unitOfWork.ServiceCenters.UpdateAsync(entity);
            await _unitOfWork.SaveChangesWithTransactionAsync();
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity =
                await _unitOfWork.ServiceCenters.GetByIdAsync(id)
                ?? throw new AppException(ErrorCode.NOT_FOUND);
            await _unitOfWork.ServiceCenters.DeleteAsync(entity);
            await _unitOfWork.SaveChangesWithTransactionAsync();
        }

        private async Task EnsureUniqueAsync(
            ServiceCenterRequest req,
            Guid? exceptId,
            CancellationToken ct
        )
        {
            if (
                req.CenterName != null
                && await _unitOfWork.ServiceCenters.ExistsNameAsync(req.CenterName, exceptId, ct)
            )
                throw new AppException(ErrorCode.HAS_EXISTED);

            if (
                req.PhoneNumber != null
                && await _unitOfWork.ServiceCenters.ExistsPhoneAsync(req.PhoneNumber, exceptId, ct)
            )
                throw new AppException(ErrorCode.HAS_EXISTED);

            if (
                req.Email != null
                && await _unitOfWork.ServiceCenters.ExistsEmailAsync(req.Email, exceptId, ct)
            )
                throw new AppException(ErrorCode.HAS_EXISTED);

            if (
                req.Address != null
                && await _unitOfWork.ServiceCenters.ExistsAddressAsync(req.Address, exceptId, ct)
            )
                throw new AppException(ErrorCode.HAS_EXISTED);
        }
    }
}
