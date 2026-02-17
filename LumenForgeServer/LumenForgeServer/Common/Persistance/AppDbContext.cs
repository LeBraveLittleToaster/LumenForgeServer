using LumenForgeServer.Billing.Domain;
using LumenForgeServer.Common.Auth.Domain;
using LumenForgeServer.Inventory.Domain;
using LumenForgeServer.Maintenance.Domain;
using LumenForgeServer.Rentals.Domain;
using Microsoft.EntityFrameworkCore;

namespace LumenForgeServer.Common.Persistance;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    // Authentication and Authorization
    public DbSet<User> Users => Set<User>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<GroupRole> GroupRoles => Set<GroupRole>();
    public DbSet<GroupUser> GroupUsers => Set<GroupUser>();
    
    // Inventory
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<MaintenanceStatus> MaintenanceStatuses => Set<MaintenanceStatus>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<Stock> Stocks => Set<Stock>();
    public DbSet<DeviceParameter> DeviceParameters => Set<DeviceParameter>();
    public DbSet<DeviceCategory> DeviceCategories => Set<DeviceCategory>();

    // Billing
    public DbSet<InvoiceStatus> InvoiceStatuses => Set<InvoiceStatus>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<PaymentStatus> PaymentStatuses => Set<PaymentStatus>();
    public DbSet<Payment> Payments => Set<Payment>();

    // Maintenance
    public DbSet<MaintenanceBacklogStatus> MaintenanceBacklogStatuses => Set<MaintenanceBacklogStatus>();
    public DbSet<MaintenanceBacklog> MaintenanceBacklogs => Set<MaintenanceBacklog>();

    // Rentals
    public DbSet<RentalStatus> RentalStatuses => Set<RentalStatus>();
    public DbSet<Rental> Rentals => Set<Rental>();
    public DbSet<RentalItem> RentalItems => Set<RentalItem>();
    public DbSet<Checklist> Checklists => Set<Checklist>();
    public DbSet<ChecklistItem> ChecklistItems => Set<ChecklistItem>();
    public DbSet<RentalReport> RentalReports => Set<RentalReport>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);
        
        // --------------------
        // Authentication and Authorization
        // --------------------
        b.Entity<User>().ToTable("users");
        b.Entity<Group>().ToTable("groups");
        b.Entity<GroupRole>().ToTable("group_roles");
        b.Entity<GroupUser>().ToTable("group_users");

        b.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.KeycloakUserId).IsUnique();
            
            e.HasMany(x => x.GroupUsers)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        b.Entity<Group>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Guid).IsUnique();
            
            e.HasMany(x => x.GroupRoles)
                .WithOne(x => x.Group)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasMany(x => x.GroupUsers)
                .WithOne(x => x.Group)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<GroupRole>(e =>
        {
            e.HasKey(x => new { x.GroupId, x.RoleId });
            e.Property(x => x.RoleId).HasConversion<int>();
            
        });
        
        b.Entity<GroupUser>(e =>
        {
            e.HasKey(x => new { x.GroupId, x.UserId });
            e.HasIndex(x => x.UserId);
        });
        
        // --------------------
        // Inventory tables
        // --------------------
        b.Entity<Vendor>().ToTable("vendor");
        b.Entity<Category>().ToTable("category");
        b.Entity<MaintenanceStatus>().ToTable("maintenance_status");
        b.Entity<Device>().ToTable("device");
        b.Entity<Stock>().ToTable("stock");
        b.Entity<DeviceParameter>().ToTable("device_parameter");
        b.Entity<DeviceCategory>().ToTable("device_category");

        b.Entity<Vendor>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(256).IsRequired();
            e.HasIndex(x => x.Guid).IsUnique();
            e.HasIndex(x => x.Name).IsUnique();
        });

        b.Entity<Category>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(256).IsRequired();
            e.HasIndex(x => x.Name).IsUnique();
            e.Property(x => x.Description).HasMaxLength(2000);
            e.HasIndex(x => x.Guid).IsUnique();
        });

        b.Entity<MaintenanceStatus>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(128).IsRequired();
            e.Property(x => x.Description).HasMaxLength(2000);
            e.HasIndex(x => x.Uuid).IsUnique();
            e.HasIndex(x => x.Name).IsUnique();
        });

        b.Entity<Device>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.SerialNumber).HasMaxLength(256).IsRequired();
            e.HasIndex(x => x.Uuid).IsUnique();
            e.HasIndex(x => x.SerialNumber).IsUnique();

            e.Property(x => x.DeviceName).HasMaxLength(512);
            e.Property(x => x.DeviceDescription).HasMaxLength(4000);
            e.Property(x => x.PhotoUrl).HasMaxLength(2000);

            e.Property(x => x.PurchasePrice).HasPrecision(18, 2);
            e.Property(x => x.PurchaseDate);

            e.HasOne(x => x.Vendor)
                .WithMany(v => v.Devices)
                .HasForeignKey(x => x.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.MaintenanceStatus)
                .WithMany(ms => ms.Devices)
                .HasForeignKey(x => x.MaintenanceStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Stock)
                .WithOne(s => s.Device)
                .HasForeignKey<Stock>(s => s.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<Stock>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Uuid).IsUnique();
            e.HasIndex(x => x.DeviceId).IsUnique(); // one-to-one

            e.Property(x => x.StockCount).HasPrecision(18, 3);
            e.Property(x => x.UnitStockType).HasConversion<string>().HasMaxLength(32).IsRequired();
        });

        b.Entity<DeviceParameter>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.ParamKey).HasMaxLength(256).IsRequired();
            e.Property(x => x.Value).HasMaxLength(4000).IsRequired();

            e.HasOne(x => x.Device)
                .WithMany(d => d.Parameters)
                .HasForeignKey(x => x.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(x => new { x.DeviceId, x.ParamKey }).IsUnique();
        });

        b.Entity<DeviceCategory>(e =>
        {
            e.HasKey(x => x.Id);

            e.HasOne(x => x.Device)
                .WithMany(d => d.DeviceCategories)
                .HasForeignKey(x => x.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Category)
                .WithMany(c => c.DeviceCategories)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(x => new { x.DeviceId, x.CategoryId }).IsUnique();
        });

        // --------------------
        // Billing tables
        // --------------------
        b.Entity<InvoiceStatus>().ToTable("invoice_status");
        b.Entity<Invoice>().ToTable("invoice");
        b.Entity<PaymentStatus>().ToTable("payment_status");
        b.Entity<Payment>().ToTable("payment");

        b.Entity<InvoiceStatus>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Uuid).IsUnique();
            e.Property(x => x.Name).HasMaxLength(128).IsRequired();
            e.Property(x => x.Description).HasMaxLength(2000);
            e.HasIndex(x => x.Name).IsUnique();
        });

        b.Entity<Invoice>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Uuid).IsUnique();

            e.Property(x => x.InvoiceNumber).HasMaxLength(128).IsRequired();
            e.HasIndex(x => x.InvoiceNumber).IsUnique();

            e.Property(x => x.SubtotalAmount).HasPrecision(18, 2);
            e.Property(x => x.TaxAmount).HasPrecision(18, 2);
            e.Property(x => x.TotalAmount).HasPrecision(18, 2);

            e.Property(x => x.CurrencyCode).HasMaxLength(3).IsRequired();
            e.Property(x => x.InvoiceDocumentUrl).HasMaxLength(2000);

            e.Property(x => x.GeneratedByUserId).HasMaxLength(128);

            e.HasOne(x => x.Rental)
                .WithMany(r => r.Invoices)
                .HasForeignKey(x => x.RentalId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.InvoiceStatus)
                .WithMany(s => s.Invoices)
                .HasForeignKey(x => x.InvoiceStatusId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<PaymentStatus>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Uuid).IsUnique();
            e.Property(x => x.Name).HasMaxLength(128).IsRequired();
            e.Property(x => x.Description).HasMaxLength(2000);
            e.HasIndex(x => x.Name).IsUnique();
        });

        b.Entity<Payment>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Uuid).IsUnique();

            e.Property(x => x.Amount).HasPrecision(18, 2);
            e.Property(x => x.CurrencyCode).HasMaxLength(3).IsRequired();
            e.Property(x => x.PaymentMethod).HasConversion<string>().HasMaxLength(32).IsRequired();
            e.Property(x => x.ProviderReference).HasMaxLength(256);

            e.HasOne(x => x.Invoice)
                .WithMany(i => i.Payments)
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.PaymentStatus)
                .WithMany(s => s.Payments)
                .HasForeignKey(x => x.PaymentStatusId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // --------------------
        // Maintenance tables
        // --------------------
        // (Your original MaintenanceDbContext didn’t set ToTable; add them if you want stable names.)
        b.Entity<MaintenanceBacklogStatus>().ToTable("maintenance_backlog_status");
        b.Entity<MaintenanceBacklog>().ToTable("maintenance_backlog");

        b.Entity<MaintenanceBacklogStatus>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Uuid).IsUnique();
            e.Property(x => x.Name).HasMaxLength(128).IsRequired();
            e.Property(x => x.Description).HasMaxLength(2000);
            e.HasIndex(x => x.Name).IsUnique();
        });

        b.Entity<MaintenanceBacklog>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Uuid).IsUnique();

            e.Property(x => x.QuantityAffected).HasPrecision(18, 3);
            e.Property(x => x.IssueSummary).HasMaxLength(2000).IsRequired();
            e.Property(x => x.IssueDescription).HasMaxLength(4000);

            e.HasOne(x => x.Stock)
                .WithMany(s => s.MaintenanceBacklogs)
                .HasForeignKey(x => x.StockId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.RentalItem)
                .WithMany(ri => ri.MaintenanceBacklogs)
                .HasForeignKey(x => x.RentalItemId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.ChecklistItem)
                .WithMany(ci => ci.MaintenanceBacklogs)
                .HasForeignKey(x => x.ChecklistItemId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.MaintenanceBacklogStatus)
                .WithMany(s => s.MaintenanceBacklogs)
                .HasForeignKey(x => x.MaintenanceBacklogStatusId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // --------------------
        // Rentals tables
        // --------------------
        b.Entity<RentalStatus>().ToTable("rental_status");
        b.Entity<Rental>().ToTable("rental");
        b.Entity<RentalItem>().ToTable("rental_item");
        b.Entity<Checklist>().ToTable("checklist");
        b.Entity<ChecklistItem>().ToTable("checklist_item");
        b.Entity<RentalReport>().ToTable("rental_report");

        b.Entity<RentalStatus>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(128).IsRequired();
            e.Property(x => x.Description).HasMaxLength(2000);
            e.HasIndex(x => x.Uuid).IsUnique();
            e.HasIndex(x => x.Name).IsUnique();
        });

        b.Entity<Rental>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Uuid).IsUnique();

            e.Property(x => x.CustomerUserId).HasMaxLength(128).IsRequired();
            e.Property(x => x.AssignedByUserId).HasMaxLength(128);
            e.Property(x => x.PickupProcessedByUserId).HasMaxLength(128);
            e.Property(x => x.DropoffProcessedByUserId).HasMaxLength(128);
            e.Property(x => x.CompletedByUserId).HasMaxLength(128);
            e.Property(x => x.ScrappedByUserId).HasMaxLength(128);

            e.HasIndex(x => x.CustomerUserId);

            e.Property(x => x.RequestTitle).HasMaxLength(512);
            e.Property(x => x.RequestDescription).HasMaxLength(4000);

            e.HasOne(x => x.RentalStatus)
                .WithMany(s => s.Rentals)
                .HasForeignKey(x => x.RentalStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.RentalReport)
                .WithOne(rr => rr.Rental)
                .HasForeignKey<RentalReport>(rr => rr.RentalId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<RentalItem>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Uuid).IsUnique();

            e.Property(x => x.Quantity).HasPrecision(18, 3);
            e.Property(x => x.ConditionNotes).HasMaxLength(4000);
            e.Property(x => x.ApprovedByUserId).HasMaxLength(128);

            e.HasOne(x => x.Rental)
                .WithMany(r => r.Items)
                .HasForeignKey(x => x.RentalId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Stock)
                .WithMany(s => s.RentalItems)
                .HasForeignKey(x => x.StockId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Checklist>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Uuid).IsUnique();

            e.Property(x => x.ChecklistType).HasConversion<string>().HasMaxLength(32).IsRequired();
            e.Property(x => x.Notes).HasMaxLength(4000);

            e.Property(x => x.GeneratedByUserId).HasMaxLength(128);
            e.Property(x => x.SignedByUserId).HasMaxLength(128);

            e.HasOne(x => x.Rental)
                .WithMany(r => r.Checklists)
                .HasForeignKey(x => x.RentalId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.SourceChecklist)
                .WithMany(x => x.DerivedChecklists)
                .HasForeignKey(x => x.SourceChecklistId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => new { x.RentalId, x.ChecklistType }).IsUnique(false);
        });

        b.Entity<ChecklistItem>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Uuid).IsUnique();

            e.Property(x => x.QuantityChecked).HasPrecision(18, 3);
            e.Property(x => x.DamagedQuantity).HasPrecision(18, 3);

            e.Property(x => x.ConditionNotes).HasMaxLength(4000);
            e.Property(x => x.DamageSummary).HasMaxLength(2000);
            e.Property(x => x.DamageDescription).HasMaxLength(4000);

            e.HasOne(x => x.Checklist)
                .WithMany(c => c.Items)
                .HasForeignKey(x => x.ChecklistId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.RentalItem)
                .WithMany(ri => ri.ChecklistItems)
                .HasForeignKey(x => x.RentalItemId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => new { x.ChecklistId, x.RentalItemId }).IsUnique();
        });

        b.Entity<RentalReport>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Uuid).IsUnique();
            e.HasIndex(x => x.RentalId).IsUnique();

            e.Property(x => x.GeneratedByUserId).HasMaxLength(128);

            e.Property(x => x.ReportSummary).HasMaxLength(4000);
            e.Property(x => x.ReportDocumentUrl).HasMaxLength(2000);
        });
    }
}
