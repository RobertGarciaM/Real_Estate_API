using DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class RealEstateDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    private readonly bool useInMemoryDatabase = false;
    public RealEstateDbContext()
    {
        useInMemoryDatabase = true;
    }
    public RealEstateDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Owner> Owners { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<PropertyImage> PropertyImages { get; set; }
    public DbSet<PropertyTrace> PropertyTraces { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            _ = useInMemoryDatabase ? optionsBuilder.UseInMemoryDatabase("RealEstateDataBase") : optionsBuilder.UseSqlServer();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.Entity<IdentityUserLogin<string>>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });

        _ = modelBuilder.Entity<Property>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18, 2)");

        _ = modelBuilder.Entity<PropertyTrace>()
      .Property(p => p.Value)
      .HasColumnType("decimal(18, 2)");

        _ = modelBuilder.Entity<PropertyTrace>()
            .Property(p => p.Tax)
            .HasColumnType("decimal(18, 2)");

    }

}