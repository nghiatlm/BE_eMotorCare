


using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class PaymentMapper : Profile
    {
        public PaymentMapper() 
        { 
        CreateMap<PaymentRequest, Payment>();
        }
    }
}
