using ExpressVoitures.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<Repair> Repairs { get; set; }
    public DbSet<Sale> Sales { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration des clés primaires et des relations
        modelBuilder.Entity<Car>()
            .HasKey(v => v.VIN);

        modelBuilder.Entity<Repair>()
            .HasKey(r => r.RepairId);

        modelBuilder.Entity<Repair>()
            .HasOne(r => r.Car)
            .WithMany(v => v.Repairs)
            .HasForeignKey(r => r.VIN);

        modelBuilder.Entity<Repair>()
            .HasOne(r => r.User)
            .WithMany(u => u.Repairs)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Sale>()
            .HasKey(s => s.SaleId);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Car)
            .WithMany(v => v.Sales)
            .HasForeignKey(s => s.VIN);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.User)
            .WithMany(u => u.Sales)
            .HasForeignKey(s => s.UserId);
    }
}
