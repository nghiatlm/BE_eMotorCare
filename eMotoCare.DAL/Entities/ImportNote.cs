
using eMotoCare.Common.Enums;


namespace eMotoCare.DAL.Entities
{
    public class ImportNote : BaseEntity
    {
        public Guid ImportNoteId { get; set; }
        public Guid BranchId { get; set; }
        public Branch? Branch { get; set; }
        public DateTime ImportDate { get; set; }
        public int TotalAmount { get; set; }
        public Status Status { get; set; }
        public string? Note { get; set; }

        public virtual ICollection<PartItem>? PartItems { get; set; }
    }
}