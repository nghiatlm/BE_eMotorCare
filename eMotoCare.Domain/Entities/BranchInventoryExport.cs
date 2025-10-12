using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMotoCare.Domain.Entities
{
    public class BranchInventoryExport
    {
        public Guid Id { get; set; }
        public Guid BranchInventoryId { get; set; }
        public Guid ExportNoteId { get; set; }

        public virtual BranchInventory? BranchInventory { get; set; }
        public virtual ExportNote? ExportNote { get; set; }
    }
}
