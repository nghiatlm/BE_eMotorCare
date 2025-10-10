
namespace eMotoCare.Domain.Entities
{
    public class ModelPartType
    {
        public Guid Id { get; set; }
        public Guid ModelId { get; set; }
        public Guid PartTypeId { get; set; }

        public virtual Model Model { get; set; }
        public virtual PartType PartType { get; set; }
    }
}