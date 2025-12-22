using System.Net;
using AutoMapper;
using eMotoCare.BO.Common.src;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.DAL;
using FirebaseAdmin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Net.payOS;
using Net.payOS.Types;

namespace eMototCare.BLL.Services.PayosServices
{
    public class PayosService : IPayosService
    {
        private readonly ILogger<PayosService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Utils _utils;
        private readonly IMapper _mapper;
        private readonly PayOS _payOS;
        private readonly IConfiguration _config;

        public PayosService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<PayosService> logger,
            Utils utils,
            PayOS payOS,
            IConfiguration config
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _utils = utils;
            _payOS = payOS;
            _config = config;
        }

        public async Task<PayOSCreatePaymentResponse?> CreatePaymentAsync(PaymentRequest request)
        {
            try
            {
                if (request == null)
                    throw new AppException("Request không được null.", HttpStatusCode.BadRequest);

                if (request.AppointmentId == Guid.Empty)
                    throw new AppException(
                        "AppointmentId không được null.",
                        HttpStatusCode.BadRequest
                    );

                if (request.Currency != EnumCurrency.VND)
                    throw new AppException("Chỉ hỗ trợ VND.", HttpStatusCode.BadRequest);

                var appointment = await _unitOfWork.Appointments.GetByIdAsync(
                    request.AppointmentId
                );
                if (appointment == null)
                    throw new AppException("Appointment không tồn tại.", HttpStatusCode.NotFound);

                var amountDec =
                    request.Amount != 0 ? request.Amount : (appointment.EVCheck?.TotalAmout ?? 0);
                if (amountDec <= 0)
                    throw new AppException(
                        "Không có giá trị thanh toán hợp lệ.",
                        HttpStatusCode.BadRequest
                    );

                var amount = (double)amountDec;

                switch (request.PaymentMethod)
                {
                    case PaymentMethod.CASH:
                        {
                            // Tạo payment & chốt SUCCESS ngay
                            var payment = _mapper.Map<Payment>(request);
                            payment.Amount = amount;
                            payment.Status = StatusPayment.SUCCESS;
                            payment.TransactionCode =
                                $"CASH-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

                            await _unitOfWork.Payments.CreateAsync(payment);

                            // Cập nhật trạng thái nghiệp vụ
                            if (appointment.EVCheck == null)
                                throw new AppException(
                                    "EVCheck không được null.",
                                    HttpStatusCode.BadRequest
                                );

                            appointment.EVCheck.Status = EVCheckStatus.COMPLETED;
                            appointment.Status = AppointmentStatus.COMPLETED;

                            await _unitOfWork.Appointments.UpdateAsync(appointment);
                            await _unitOfWork.SaveAsync();

                            // CASH không có checkoutUrl
                            return null;
                        }

                    case PaymentMethod.PAY_OS_CENTER:
                        {
                            // Tạo payment pending
                            var orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                            var payment = _mapper.Map<Payment>(request);
                            payment.Amount = amount;
                            payment.Status = StatusPayment.PENDING;
                            payment.TransactionCode = orderCode.ToString();

                            await _unitOfWork.Payments.CreateAsync(payment);
                            await _unitOfWork.SaveAsync();
                            var returnUrl = request.SuccessUrl
                                ?? "https://emotocare.vercel.app/payment-success";
                            var cancelUrl =
                            request.CancelUrl
                                ?? "https://emotocare.vercel.app/payment-failed";

                            var item = new ItemData(
                                $"Appointment - {appointment.Id}",
                                1,
                                (int)Math.Round(amount)
                            );

                            var items = new List<ItemData> { item };

                            var paymentData = new PaymentData(
                                orderCode,
                                (int)Math.Round(amount),
                                "Thanh toán bảo dưỡng xe",
                                items,
                                cancelUrl,
                                returnUrl
                            );

                            var createPayment = await _payOS.createPaymentLink(paymentData);
                            return new PayOSCreatePaymentResponse
                            {
                                CheckoutUrl = createPayment.checkoutUrl,
                                TransactionCode = orderCode.ToString(),
                            };
                        }
                    case PaymentMethod.PAY_OS_APP:
                        {
                            var orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                            var payment = _mapper.Map<Payment>(request);
                            payment.Amount = amount;
                            payment.Status = StatusPayment.PENDING;
                            payment.TransactionCode = orderCode.ToString();

                            await _unitOfWork.Payments.CreateAsync(payment);
                            await _unitOfWork.SaveAsync();
                            var returnUrl = request.SuccessUrl
                                ?? "https://emotocare.vercel.app/payment-success";
                            var cancelUrl = request.CancelUrl
                                ?? "https://emotocare.vercel.app/payment-failed";

                            var item = new ItemData(
                                $"Appointment - {appointment.Id}",
                                1,
                                (int)Math.Round(amount)
                            );

                            var items = new List<ItemData> { item };

                            var paymentData = new PaymentData(
                                orderCode,
                                (int)Math.Round(amount),
                                "Thanh toán bảo dưỡng xe",
                                items,
                                cancelUrl,
                                returnUrl
                            );

                            var createPayment = await _payOS.createPaymentLink(paymentData);

                            // Update appointment status for PAY_OS_APP
                            if (appointment.EVCheck == null)
                                throw new AppException(
                                    "EVCheck không được null.",
                                    HttpStatusCode.BadRequest
                                );

                            //appointment.EVCheck.Status = EVCheckStatus.REPAIR_COMPLETED;
                            appointment.Status = AppointmentStatus.WAITING_FOR_PAYMENT;

                            await _unitOfWork.Appointments.UpdateAsync(appointment);
                            await _unitOfWork.SaveAsync();

                            return new PayOSCreatePaymentResponse
                            {
                                CheckoutUrl = createPayment.checkoutUrl,
                                TransactionCode = orderCode.ToString(),
                            };
                        }

                    default:
                        throw new AppException(
                            "PaymentMethod không hợp lệ.",
                            HttpStatusCode.BadRequest
                        );
                }
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException(
                    "Đã xảy ra lỗi máy chủ.",
                    HttpStatusCode.InternalServerError
                );
            }
        }

        public async Task<bool> VerifyPaymentAsync(WebhookType type)
        {
            try
            {
                var data = _payOS.verifyPaymentWebhookData(type);

                if (data == null)
                {
                    _logger.LogWarning("PayOS webhook returned null payload.");
                    return true;
                }

                var orderCode = data.orderCode.ToString();
                if (string.IsNullOrEmpty(orderCode))
                {
                    _logger.LogWarning("PayOS webhook payload missing orderCode: {@Payload}", data);
                    return true;
                }

                var payment = await _unitOfWork.Payments.GetByTransactionCodeAsync(orderCode);
                if (payment == null)
                {
                    _logger.LogWarning("No payment found for orderCode {OrderCode}", orderCode);
                    return true;
                }

                // Prefer the Appointment instance that came with the payment query (if present)
                // to avoid loading a second instance with the same key into the DbContext.
                var appointmentId = payment.AppointmentId;
                VehicleStage vehicleStage = null;
                // if (appointment == null && payment.AppointmentId != Guid.Empty)
                // {
                //     appointment = await _unitOfWork.Appointments.GetByIdAsync(
                //         payment.AppointmentId
                //     );
                // }
                if (appointmentId == null) throw new AppException("Payment không có AppointmentId.", HttpStatusCode.BadRequest);
                var appointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId);

                // Load vehicleStage only if appointment exists and has a valid stage
                if (appointment != null)
                {
                    vehicleStage = appointment.VehicleStage;
                }

                if (data.code == "00")
                {
                    payment.Status = StatusPayment.SUCCESS;
                    if (appointment != null)
                    {
                        appointment.Status = AppointmentStatus.COMPLETED;
                        if (appointment.EVCheck != null)
                        {
                            appointment.EVCheck.Status = EVCheckStatus.COMPLETED;
                        }
                        // Only update vehicleStage if it was successfully loaded
                        if (vehicleStage != null && vehicleStage.Id != Guid.Empty)
                        {
                            vehicleStage.Status = VehicleStageStatus.COMPLETED;
                        }
                    }
                }
                else
                {
                    payment.Status = StatusPayment.CANCELLED;
                    if (appointment != null)
                        appointment.Status = AppointmentStatus.PAYMENT_FAILED;
                }

                // If payment.Appointment is a different instance than the tracked appointment
                // returned by FindByIdAsync, clear the navigation to avoid EF Core trying to
                // attach two instances with the same key.
                if (
                    payment.Appointment != null
                    && appointment != null
                    && !ReferenceEquals(payment.Appointment, appointment)
                )
                {
                    payment.Appointment = null;
                }

                // Only update vehicleStage if it was successfully loaded and has valid ID
                if (vehicleStage != null && vehicleStage.Id != Guid.Empty)
                {
                    await _unitOfWork.VehicleStages.UpdateAsync(vehicleStage);
                }
                await _unitOfWork.Payments.UpdateAsync(payment);
                if (appointment != null)
                    await _unitOfWork.Appointments.UpdateAsync(appointment);

                var result = await _unitOfWork.SaveAsync();
                return result > 0;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[VerifyPaymentAsync] Error while processing webhook.");
                throw new AppException(
                    "An error occurred while verifying the payment.",
                    HttpStatusCode.InternalServerError
                );
            }
        }
    }
}
