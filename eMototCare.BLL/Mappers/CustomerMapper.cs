

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class CustomerMapper : Profile
    {
        public CustomerMapper()
        {
            CreateMap<CustomerRequest, Customer>();
        }
    }
}
