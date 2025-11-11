

using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Responses
{
    public class ServiceCenterInventoryResponse
    {
        public Guid Id { get; set; }
        public string ServiceCenterInventoryName { get; set; }
        public ServiceCenterResponse? ServiceCenter { get; set; }
        public Status Status { get; set; }
        public ICollection<PartItemResponse>? PartItems { get; set; }
    }
}
