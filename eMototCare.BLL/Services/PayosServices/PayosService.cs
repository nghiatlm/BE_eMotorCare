

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
                    CustomerID = evCheck.Appointment.CustomerId
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
    }
}
