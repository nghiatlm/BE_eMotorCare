using eMotoCare.Domain.Common;
using eMotoCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.Infrastructure.Context
{
    public class DBContextMotoCare : DbContext
    {
        public DBContextMotoCare() { }

        public DBContextMotoCare(DbContextOptions<DBContextMotoCare> options) : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<BatteryCheck> BatteryChecks { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<BranchInventory> BranchInventories { get; set; }
        public DbSet<BranchInventoryExport> BranchInventoryExports { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignDetail> CampaignDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<EVCheck> EVChecks { get; set; }
        public DbSet<EVCheckDetail> EVCheckDetails { get; set; }
        public DbSet<ExportNote> ExportNotes { get; set; }
        public DbSet<ImportNote> ImportNotes { get; set; }
        public DbSet<MaintenaceStage> MaintenaceStages { get; set; }
        public DbSet<MaintenaceStageDetail> MaintenaceStageDetails { get; set; }
        public DbSet<MaintenancePlan> MaintenancePlans { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<ModelPartType> ModelPartTypes { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<PartItem> PartItems { get; set; }
        public DbSet<PartType> PartTypes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PriceService> PriceServices { get; set; }
        public DbSet<RMA> RMAs { get; set; }    
        public DbSet<RMADetail> RMADetails { get; set; }
        public DbSet<ServiceCenter> ServiceCenters { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }    
        public DbSet<VehiclePartItem> VehiclePartItems { get; set; }
        public DbSet<VehicleStage> VehicleStages { get; set; }

        private static readonly TimeZoneInfo _vnZone =
        TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        // Trả về DateTime ở múi VN
        private DateTime GetCurrentVnTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _vnZone);
        }
        public override int SaveChanges()
        {
            ApplyTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            ApplyTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void ApplyTimestamps()
        {
            DateTime now = GetCurrentVnTime();

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(x => x.CreatedAt).IsModified = false;
                    entry.Entity.UpdatedAt = now;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType.IsEnum);

                foreach (var property in properties)
                {
                    modelBuilder.Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion<string>();
                }
            }

            modelBuilder.Entity<Branch>()
                .HasOne(b => b.ManageBy)
                .WithOne()
                .HasForeignKey<Branch>(b => b.ManageById);

            modelBuilder.Entity<VehiclePartItem>()
                .HasOne(vpi => vpi.PartItem)
                .WithMany(pi => pi.VehiclePartItems)
                .HasForeignKey(vpi => vpi.PartItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VehiclePartItem>()
                .HasOne(vpi => vpi.ReplaceFor)
                .WithMany()
                .HasForeignKey(vpi => vpi.ReplaceForId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

    }
}