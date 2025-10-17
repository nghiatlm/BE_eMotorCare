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
            CreateMap<StaffRequest, Staff>().ForMember(d => d.Id, opt => opt.Ignore());

            CreateMap<Staff, StaffResponse>();
        }
    }
}
