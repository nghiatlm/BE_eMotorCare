using AutoMapper;
using eMotoCare.Common.Models.Requests;
using eMotoCare.Common.Models.Responses;
using eMotoCare.DAL.Entities;

namespace eMotoCare.BLL.Mappers
{
    public class StaffMapper : Profile
    {
        public StaffMapper()
        {
            CreateMap<StaffRequest, Staff>()
                .ForMember(d => d.StaffId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore());

            CreateMap<Staff, StaffResponse>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.StaffId));
        }
    }
}
