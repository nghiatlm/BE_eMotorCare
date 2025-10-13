using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMotoCare.Domain.Entities
{
    public class BranchInventoryExport
    {
        public Guid BranchInventoryExportId { get; set; }
        public Guid BranchInventoryId { get; set; }
        public BranchInventory BranchInventory { get; set; }
        public Guid ExportNoteId { get; set; }
        public ExportNote ExportNote { get; set; }

    }
}
