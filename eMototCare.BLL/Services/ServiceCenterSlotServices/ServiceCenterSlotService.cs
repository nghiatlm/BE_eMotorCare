using System.Net;
using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
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
                await _unitOfWork.ServiceCenterSlot.GetByServiceCenterAsync(serviceCenterId)
                ?? throw new AppException("Không tìm thấy ServiceCenter", HttpStatusCode.NotFound);

            var items = await _serviceCenterSlotRepository.GetByServiceCenterAsync(serviceCenterId);
            return _mapper.Map<List<ServiceCenterSlotResponse>>(items);
        }

        public async Task<Guid> CreateAsync(Guid serviceCenterId, ServiceCenterSlotRequest req)
        {
            var center =
                await _unitOfWork.ServiceCenters.GetByIdAsync(serviceCenterId)
                ?? throw new AppException("Không tìm thấy ServiceCenter", HttpStatusCode.NotFound);

            if (req.Capacity < 1)
                throw new AppException("Capacity phải >= 1", HttpStatusCode.BadRequest);
            req.Date = AlignDateToDayOfWeek(req.Date, req.DayOfWeek);
            var duplicated = await _serviceCenterSlotRepository.ExistsSlotAsync(
                serviceCenterId,
                req.Date,
                req.DayOfWeek,
                req.SlotTime
            );
            if (duplicated)
                throw new AppException("Slot đã tồn tại", HttpStatusCode.Conflict);

            var now = DateTime.UtcNow.AddHours(7);
            var today = DateOnly.FromDateTime(now.Date);

            if (req.Date < today)
            {
                throw new AppException(
                    "Ngày tạo khung giờ phải từ hôm nay trở đi.",
                    HttpStatusCode.BadRequest
                );
            }

            if (req.Date == today)
            {
                var slotStart = GetStartTime(req.SlotTime);
                if (slotStart <= now.TimeOfDay)
                {
                    throw new AppException(
                        "Đã quá thời gian tạo khung giờ này.",
                        HttpStatusCode.BadRequest
                    );
                }
            }

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

            if (req.Capacity < 1)
                throw new AppException("Capacity phải >= 1", HttpStatusCode.BadRequest);

            if (req.Capacity < 1)
                throw new AppException("Capacity phải >= 1", HttpStatusCode.BadRequest);
            req.Date = AlignDateToDayOfWeek(req.Date, req.DayOfWeek);
            var allOfCenter = await _serviceCenterSlotRepository.GetByServiceCenterAsync(
                serviceCenterId
            );
            var duplicated = allOfCenter.Any(x =>
                x.Id != slotId
                && x.IsActive
                && (x.Date == req.Date || (x.Date == default && x.DayOfWeek == req.DayOfWeek))
                && x.SlotTime == req.SlotTime
            );
            if (duplicated)
                throw new AppException("Slot đã tồn tại", HttpStatusCode.Conflict);
            var now = DateTime.UtcNow.AddHours(7);
            var today = DateOnly.FromDateTime(now.Date);

            // Slot cũ đã quá giờ không cho sửa
            var oldSlotStart = GetStartTime(slot.SlotTime);
            if (slot.Date < today || (slot.Date == today && oldSlotStart <= now.TimeOfDay))
            {
                throw new AppException(
                    "Đã quá thời gian sửa khung giờ này.",
                    HttpStatusCode.BadRequest
                );
            }
            if (req.Date < today)
            {
                throw new AppException(
                    "Ngày cập nhật khung giờ phải từ hôm nay trở đi.",
                    HttpStatusCode.BadRequest
                );
            }
            if (req.Date == today)
            {
                var newSlotStart = GetStartTime(req.SlotTime);
                if (newSlotStart <= now.TimeOfDay)
                {
                    throw new AppException(
                        "Đã quá thời gian cập nhật khung giờ này.",
                        HttpStatusCode.BadRequest
                    );
                }
            }

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
            var todaySlots = all.Where(s => s.IsActive && s.Date == date).ToList();

            if (todaySlots.Count == 0)
            {
                todaySlots = all.Where(s =>
                        s.IsActive && s.Date == default && s.DayOfWeek == (DayOfWeeks)dow
                    )
                    .ToList();
            }

            var result = new List<(ServiceCenterSlotResponse, int)>();
            foreach (var s in todaySlots)
            {
                //đếm theo (Date, SlotTime) — không dùng slotId
                var booked = await _serviceCenterSlotRepository.CountBookingsAsync(
                    serviceCenterId,
                    date,
                    s.SlotTime
                );

                var remaining = Math.Max(0, s.Capacity - booked);
                result.Add((_mapper.Map<ServiceCenterSlotResponse>(s), remaining));
            }
            return result;
        }

        private static DateOnly AlignDateToDayOfWeek(DateOnly date, DayOfWeeks dow)
        {
            var target = (DayOfWeek)dow;
            var cur = date.ToDateTime(TimeOnly.MinValue).DayOfWeek;
            int delta = ((int)target - (int)cur + 7) % 7;
            return date.AddDays(delta);
        }

        private TimeSpan GetStartTime(SlotTime slot)
        {
            return slot switch
            {
                SlotTime.H07_08 => new TimeSpan(7, 0, 0),
                SlotTime.H08_09 => new TimeSpan(8, 0, 0),
                SlotTime.H09_10 => new TimeSpan(9, 0, 0),
                SlotTime.H10_11 => new TimeSpan(10, 0, 0),
                SlotTime.H11_12 => new TimeSpan(11, 0, 0),
                SlotTime.H13_14 => new TimeSpan(13, 0, 0),
                SlotTime.H14_15 => new TimeSpan(14, 0, 0),
                SlotTime.H15_16 => new TimeSpan(15, 0, 0),
                SlotTime.H16_17 => new TimeSpan(16, 0, 0),
                SlotTime.H17_18 => new TimeSpan(17, 0, 0),
                _ => new TimeSpan(23, 59, 59),
            };
        }
    }
}
