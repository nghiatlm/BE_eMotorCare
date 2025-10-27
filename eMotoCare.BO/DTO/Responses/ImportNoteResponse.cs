

using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Responses
{
    public class ImportNoteResponse 
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime ImportDate { get; set; }
        public string ImportFrom { get; set; }
        public string? Supplier { get; set; }
        public ImportType Type { get; set; }
        public decimal? TotalAmout { get; set; }
        public StaffResponse? ImportBy { get; set; }
        public ServiceCenterResponse? ServiceCenter { get; set; }
    }
}
