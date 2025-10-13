

using AutoMapper;
using eMotoCare.Application.DTOs;
using eMotoCare.Domain.Entities;

namespace eMotoCare.Application.Mapper
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {

            CreateMap<RegisterRequest, Account>();

        }
    }
}
