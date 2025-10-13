
namespace eMotoCare.DAL.Entities
{
    public class BranchInventory
    {
        public Guid BranchInventoryId { get; set; }
        public Guid BranchId { get; set; }
        public Branch? Branch { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual ICollection<PartItem>? PartItems { get; set; }
        public virtual ICollection<BranchInventoryExport>? BranchInventoryExports { get; set; }

    }
}