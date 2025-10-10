
namespace eMotoCare.Domain.Entities
{
    public class Model
    {
        public Guid Id { get; set; }
        public string ModelName { get; set; }
        public Guid MaintenancePlanId { get; set; }
        public string? Description { get; set; }

        public virtual MaintenancePlan? MaintenancePlan { get; set; }
    }
}