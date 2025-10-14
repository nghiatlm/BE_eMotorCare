using AutoMapper;
using eMotoCare.Common.Models.Requests;
using eMotoCare.Common.Models.Responses;
using eMotoCare.DAL.Entities;

namespace eMotoCare.BLL.Mappers
{
    public class BranchMapper : Profile
    {
        public BranchMapper()
        {
            CreateMap<BranchRequest, Branch>()
                .ForMember(d => d.BranchId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore());

            CreateMap<Branch, BranchResponse>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.BranchId));
        }
    }
}
