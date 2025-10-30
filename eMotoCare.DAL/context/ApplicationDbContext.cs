using eMotoCare.BO.Common;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace eMotoCare.DAL.context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<BatteryCheck> BatteryChecks { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignDetail> CampaignDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<EVCheck> EVChecks { get; set; }
        public DbSet<EVCheckDetail> EVCheckDetails { get; set; }
        public DbSet<ExportNote> ExportNotes { get; set; }
        public DbSet<ImportNote> ImportNotes { get; set; }
        public DbSet<MaintenancePlan> MaintenancePlans { get; set; }
        public DbSet<MaintenanceStage> MaintenanceStages { get; set; }
        public DbSet<MaintenanceStageDetail> MaintenanceStageDetails { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<ModelPartType> ModelPartTypes { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<PartItem> PartItems { get; set; }
        public DbSet<PartType> PartTypes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<RMA> RMAs { get; set; }
        public DbSet<PriceService> PriceServices { get; set; }
        public DbSet<RMADetail> RMADetails { get; set; }
        public DbSet<ServiceCenter> ServiceCenters { get; set; }
        public DbSet<ServiceCenterInventory> ServiceCenterInventorys { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehiclePartItem> VehiclePartItems { get; set; }
        public DbSet<VehicleStage> VehicleStages { get; set; }
        public DbSet<ServiceCenterSlot> ServiceCenterSlots { get; set; }
        private static readonly TimeZoneInfo _vnZone = TimeZoneInfo.FindSystemTimeZoneById(
            "SE Asia Standard Time"
        );

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
            CancellationToken cancellationToken = default
        )
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

        private class EnumArrayToStringConverter<TEnum> : ValueConverter<TEnum[], string>
            where TEnum : struct, Enum
        {
            public EnumArrayToStringConverter()
                : base(
                    v => string.Join(",", v.Select(x => x.ToString())),
                    v =>
                        v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => Enum.Parse<TEnum>(x))
                            .ToArray()
                ) { }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<MaintenancePlan>()
                .Property(x => x.Unit)
                .HasConversion(new EnumArrayToStringConverter<MaintenanceUnit>());

            modelBuilder
                .Entity<MaintenanceStageDetail>()
                .Property(x => x.ActionType)
                .HasConversion(new EnumArrayToStringConverter<ActionType>());
            modelBuilder
                .Entity<ServiceCenterSlot>()
                .Property(s => s.DayOfWeek)
                .HasConversion<string>()
                .HasColumnType("varchar(16)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
