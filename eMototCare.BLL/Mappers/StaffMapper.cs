using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class StaffMapper : Profile
    {
        public StaffMapper()
        {
            CreateMap<StaffRequest, Staff>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.AccountId, opt => opt.Ignore())
                .ForMember(d => d.StaffCode, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
                .ForMember(d => d.Account, opt => opt.Ignore())
                .ForMember(d => d.ServiceCenter, opt => opt.Ignore())
                .ForMember(
                    d => d.ServiceCenterId,
                    opt =>
                        opt.Condition(
                            (src, dest) =>
                                src.ServiceCenterId.HasValue
                                && src.ServiceCenterId.Value != Guid.Empty
                        )
                )
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Staff, StaffResponse>();
        }
    }
}
