using AutoMapper;
using eMotoCare.Common.Models.Requests;
using eMotoCare.Common.Models.Responses;
using eMotoCare.DAL.Entities;

namespace eMotoCare.BLL.Mappers
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            CreateMap<RegisterRequest, Account>();
            CreateMap<Account, UserResponse>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.AccountId));
        }
    }
}
