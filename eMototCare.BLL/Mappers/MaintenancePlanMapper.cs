﻿

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class MaintenancePlanMapper : Profile
    {
        public MaintenancePlanMapper()
        {
            CreateMap<MaintenancePlanRequest, MaintenancePlan>();
            CreateMap<MaintenancePlan, MaintenancePlanResponse>();
            CreateMap<MaintenancePlanUpdateRequest, MaintenancePlan>();
        }
    }
}
