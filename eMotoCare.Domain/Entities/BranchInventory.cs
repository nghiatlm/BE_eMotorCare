
namespace eMotoCare.Domain.Entities
{
    public class BranchInventory
    {
        public Guid Id { get; set; }
        public Guid BranchId { get; set; }
        public Guid PartItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual Branch? Branch { get; set; }
        public virtual PartItem? PartItem { get; set; }

    }
}