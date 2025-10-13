

using AutoMapper;
using eMotoCare.Common.Models;
using eMotoCare.DAL.Entities;

namespace eMotoCare.BLL.Mappers
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {

            CreateMap<RegisterRequest, Account>();

        }
    }
}
