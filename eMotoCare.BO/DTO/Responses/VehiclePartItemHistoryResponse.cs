

namespace eMotoCare.BO.DTO.Responses
{
    public class VehiclePartItemHistoryResponse
    {
        public Guid Id { get; set; }              
        public DateTime InstallDate { get; set; }
        public PartItemResponse? NewPartItem { get; set; }    
        public PartItemResponse? ReplacedForPartItem { get; set; } 
    }

}
