﻿using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            CreateMap<AccountRequest, Account>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Password, opt => opt.Ignore());

            CreateMap<Account, AccountResponse>();
        }
    }
}
