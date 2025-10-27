


using eMotoCare.BO.Enums;


namespace eMotoCare.BO.DTO.Responses
{
    public class PartResponse
    {
        public Guid Id { get; set; }
        public PartTypeResponse? PartType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string? Image { get; set; }
        public PartStatus Status { get; set; }
    }
}
