
namespace eMotoCare.Domain.Entities
{
    public class ModelPartType
    {
        public Guid ModelPartTypeId { get; set; }
        public Guid ModelId { get; set; }
        public Guid PartTypeId { get; set; }

        public Model Model { get; set; }
        public PartType PartType { get; set; }
    }
}