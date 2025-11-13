

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class ServiceCenterInventoryMapper : Profile
    {
        public ServiceCenterInventoryMapper() 
        {

            CreateMap<ServiceCenterInventory, ServiceCenterInventoryResponse>();
            CreateMap<ServiceCenterInventoryRequest, ServiceCenterInventory>();
            CreateMap<ServiceCenterInventoryUpdateRequest, ServiceCenterInventory>();

        }
    }
}
