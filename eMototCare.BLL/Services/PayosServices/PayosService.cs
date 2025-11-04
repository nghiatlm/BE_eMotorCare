

using AutoMapper;
using eMotoCare.BO.Common.src;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.DAL;
using FirebaseAdmin;
using Microsoft.Extensions.Logging;
using Net.payOS;
using Net.payOS.Types;
using System.Net;


namespace eMototCare.BLL.Services.PayosServices
{
    public class PayosService : IPayosService
    {
        private readonly ILogger<PayosService> _logger;
        private readonly IUnitOfWork _uow;
        private readonly Utils _utils;
        private readonly IMapper _mapper;
        private readonly PayOS _payOS;

        public PayosService(IUnitOfWork uow, IMapper mapper, ILogger<PayosService> logger,
                            Utils utils, PayOS payOS)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _utils = utils;
            _payOS = payOS;
        }

        public async Task<string> CreatePaymentAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new AppException("Id không được null.", HttpStatusCode.BadRequest);

                var evCheck = await _uow.EVChecks.GetByIdAsync(id);
                if (evCheck == null)
                    throw new AppException("EVCheck không tồn tại.", HttpStatusCode.NotFound);

                if (evCheck.TotalAmout == null || evCheck.TotalAmout <= 0)
                    throw new AppException("EVCheck không có giá trị thanh toán.", HttpStatusCode.BadRequest);


                var orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                PaymentRequest paymentRequest = new PaymentRequest
                {
                    Amount = evCheck.TotalAmout ?? 0,
                    Currency = EnumCurrency.VND,
                    PaymentMethod = PaymentMethod.PAYOS,
                    AppointmentId = evCheck.AppointmentId,
                };
                var payment = _mapper.Map<Payment>(paymentRequest);
                payment.Status = StatusPayment.PENDING;
                payment.TransactionCode = orderCode.ToString();
                await _uow.Payments.CreateAsync(payment);
                await _uow.SaveAsync();
                var item = new ItemData(
                                        $"EV Check - {evCheck.Id}",
                                        1,
                                        (int)(evCheck.TotalAmout ?? 0)
                                        );
                var items = new List<ItemData> { item };


                var paymentData = new PaymentData(
                                                orderCode,
                                                (int)(evCheck.TotalAmout ?? 0),
                                                "Đơn hàng phục vụ học tập",
                                                items,
                                                "https://modernestate.vercel.app/payment-failure",
                                                "https://modernestate.vercel.app/payment-success"
                                                );
                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
                return createPayment.checkoutUrl;
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException("Đã xảy ra lỗi máy chủ.", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<bool> VerifyPaymentAsync(WebhookType type)
        {
            try
            {
                var data = _payOS.verifyPaymentWebhookData(type);
                var transaction = await _uow.Payments.GetByTransactionCodeAsync(data.orderCode.ToString());
                if (transaction == null)
                {
                    _logger.LogWarning(
                                   "No transaction found for order code {OrderCode}, skipping webhook processing.",
                                   data.orderCode
                               );
                    return true;
                }

                if (data.code == "00")
                {
                    _logger.LogInformation("Payment verified successfully: {Code}", data.code);
                    if (transaction.Appointment?.EVCheck == null)
                        throw new AppException("EVCheck không được null.", HttpStatusCode.BadRequest);
                    transaction.Appointment.EVCheck.Status = EVCheckStatus.COMPLETED;
                    transaction.Status = StatusPayment.SUCCESS;
                    await _uow.Payments.UpdateAsync(transaction);
                    _logger.LogInformation("EVCheck marked as COMPLETED for transaction {TransactionId}", transaction.Id);

                    await _uow.SaveAsync();
                }
                else
                {
                    _logger.LogWarning("Payment failed or canceled: {Code}", data.code);

                    transaction.Status = StatusPayment.FAILED;
                }

                await _uow.Payments.UpdateAsync(transaction);
                await _uow.SaveAsync();

                return data.code == "00";
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
