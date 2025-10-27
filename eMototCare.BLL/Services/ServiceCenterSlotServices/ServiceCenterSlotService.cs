using System.Net;
using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Exceptions;
using eMotoCare.DAL;
using eMotoCare.DAL.Repositories.ServiceCenterSlotRepository;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.ServiceCenterSlotServices
{
    public class ServiceCenterSlotService : IServiceCenterSlotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceCenterSlotRepository _serviceCenterSlotRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceCenterSlotService> _logger;

        public ServiceCenterSlotService(
            IUnitOfWork unitOfWork,
            IServiceCenterSlotRepository serviceCenterSlotRepository,
            IMapper mapper,
            ILogger<ServiceCenterSlotService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _serviceCenterSlotRepository = serviceCenterSlotRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ServiceCenterSlotResponse>> GetAllAsync(Guid serviceCenterId)
        {
            var center =
                await _unitOfWork.ServiceCenters.GetByIdAsync(serviceCenterId)
                ?? throw new AppException("Không tìm thấy ServiceCenter", HttpStatusCode.NotFound);

            var items = await _serviceCenterSlotRepository.GetByServiceCenterAsync(serviceCenterId);
            return _mapper.Map<List<ServiceCenterSlotResponse>>(items);
        }

        public async Task<Guid> CreateAsync(Guid serviceCenterId, ServiceCenterSlotRequest req)
        {
            var center =
                await _unitOfWork.ServiceCenters.GetByIdAsync(serviceCenterId)
                ?? throw new AppException("Không tìm thấy ServiceCenter", HttpStatusCode.NotFound);

            if (req.EndTime <= req.StartTime)
                throw new AppException("Thời gian slot không hợp lệ", HttpStatusCode.BadRequest);

            if (req.Capacity < 1)
                throw new AppException("Capacity phải >= 1", HttpStatusCode.BadRequest);

            if (
                await _serviceCenterSlotRepository.HasOverlapAsync(
                    serviceCenterId,
                    req.DayOfWeek,
                    req.StartTime,
                    req.EndTime
                )
            )
                throw new AppException("Slot bị trùng thời gian", HttpStatusCode.Conflict);

            var entity = _mapper.Map<ServiceCenterSlot>(req);
            entity.Id = Guid.NewGuid();
            entity.ServiceCenterId = serviceCenterId;

            await _serviceCenterSlotRepository.CreateAsync(entity);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation(
                "Created slot {SlotId} for center {CenterId}",
                entity.Id,
                serviceCenterId
            );
            return entity.Id;
        }

        public async Task UpdateAsync(
            Guid serviceCenterId,
            Guid slotId,
            ServiceCenterSlotRequest req
        )
        {
            var slot =
                await _serviceCenterSlotRepository.FindByIdAsync(slotId)
                ?? throw new AppException("Không tìm thấy Slot", HttpStatusCode.NotFound);

            if (slot.ServiceCenterId != serviceCenterId)
                throw new AppException("Slot không thuộc trung tâm này", HttpStatusCode.BadRequest);

            if (req.EndTime <= req.StartTime)
                throw new AppException("Thời gian slot không hợp lệ", HttpStatusCode.BadRequest);

            if (req.Capacity < 1)
                throw new AppException("Capacity phải >= 1", HttpStatusCode.BadRequest);

            if (
                await _serviceCenterSlotRepository.HasOverlapAsync(
                    serviceCenterId,
                    req.DayOfWeek,
                    req.StartTime,
                    req.EndTime,
                    excludeId: slotId
                )
            )
                throw new AppException("Slot bị trùng thời gian", HttpStatusCode.Conflict);

            _mapper.Map(req, slot);

            await _serviceCenterSlotRepository.UpdateAsync(slot);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Updated slot {SlotId}", slotId);
        }

        public async Task DeleteAsync(Guid serviceCenterId, Guid slotId)
        {
            var slot =
                await _serviceCenterSlotRepository.FindByIdAsync(slotId)
                ?? throw new AppException("Không tìm thấy Slot", HttpStatusCode.NotFound);

            if (slot.ServiceCenterId != serviceCenterId)
                throw new AppException("Slot không thuộc trung tâm này", HttpStatusCode.BadRequest);

            await _serviceCenterSlotRepository.DeleteAsync(slot);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Deleted slot {SlotId}", slotId);
        }

        public async Task<
            List<(ServiceCenterSlotResponse Slot, int Remaining)>
        > GetAvailabilityAsync(Guid serviceCenterId, DateOnly date)
        {
            var center =
                await _unitOfWork.ServiceCenters.GetByIdAsync(serviceCenterId)
                ?? throw new AppException("Không tìm thấy ServiceCenter", HttpStatusCode.NotFound);

            var dow = date.ToDateTime(TimeOnly.MinValue).DayOfWeek;
            var all = await _serviceCenterSlotRepository.GetByServiceCenterAsync(serviceCenterId);
            var todaySlots = all.Where(s => s.IsActive && s.DayOfWeek == dow).ToList();

            var result = new List<(ServiceCenterSlotResponse, int)>();
            foreach (var s in todaySlots)
            {
                var booked = await _serviceCenterSlotRepository.CountBookingsAsync(
                    serviceCenterId,
                    s.Id,
                    date
                );
                var remaining = Math.Max(0, s.Capacity - booked);
                result.Add((_mapper.Map<ServiceCenterSlotResponse>(s), remaining));
            }
            return result;
        }
    }
}
