
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("program_model")]
    public class ProgramModel
    {
        [Column("program_id")]
        public Guid ProgramId { get; set; }

        [ForeignKey(nameof(ProgramId))]
        public virtual Program? Program { get; set; }

        [Column("vehicle_model_id")]
        public Guid VehicleModelId { get; set; }

        [ForeignKey(nameof(VehicleModelId))]
        public virtual Model? VehicleModel { get; set; }
    }
}