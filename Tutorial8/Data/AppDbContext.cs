using Microsoft.EntityFrameworkCore;
using Tutorial8.Models;

namespace Tutorial8.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<PC> PCs { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<PCComponent> PCComponents { get; set; }
    public DbSet<ComponentType> ComponentTypes { get; set; }
    public DbSet<ComponentManufacturer> ComponentManufacturers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PC>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(50);
            e.Property(x => x.Weight).IsRequired().HasColumnType("float(5)");
            e.Property(x => x.Warranty).IsRequired();
            e.Property(x => x.CreatedAt).IsRequired().HasColumnType("datetime");
            e.Property(x => x.Stock).IsRequired();
        });

        modelBuilder.Entity<ComponentManufacturer>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Abbreviation).IsRequired().HasMaxLength(30);
            e.Property(x => x.FullName).IsRequired().HasMaxLength(300);
            e.Property(x => x.FoundationDate).IsRequired().HasColumnType("date");
        });

        modelBuilder.Entity<ComponentType>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Abbreviation).IsRequired().HasMaxLength(30);
            e.Property(x => x.Name).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<Component>(e =>
        {
            e.HasKey(x => x.Code);
            e.Property(x => x.Code).IsRequired().HasColumnType("char(10)");
            e.Property(x => x.Name).IsRequired().HasMaxLength(300);
            e.Property(x => x.Description).IsRequired().HasColumnType("nvarchar(max)");

            e.HasOne(x => x.Manufacturer)
                .WithMany(m => m.Components)
                .HasForeignKey(x => x.ComponentManufacturersId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Type)
                .WithMany(t => t.Components)
                .HasForeignKey(x => x.ComponentTypesId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PCComponent>(e =>
        {
            e.HasKey(x => new { x.PCId, x.ComponentCode });
            e.Property(x => x.ComponentCode).HasColumnType("char(10)");
            e.Property(x => x.Amount).IsRequired();

            e.HasOne(x => x.PC)
                .WithMany(p => p.PCComponents)
                .HasForeignKey(x => x.PCId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Component)
                .WithMany(c => c.PCComponents)
                .HasForeignKey(x => x.ComponentCode)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed data
        modelBuilder.Entity<ComponentType>().HasData(
            new ComponentType { Id = 1, Abbreviation = "CPU", Name = "Processor" },
            new ComponentType { Id = 2, Abbreviation = "GPU", Name = "Graphics Card" },
            new ComponentType { Id = 3, Abbreviation = "RAM", Name = "Memory" }
        );

        modelBuilder.Entity<ComponentManufacturer>().HasData(
            new ComponentManufacturer { Id = 1, Abbreviation = "AMD", FullName = "Advanced Micro Devices", FoundationDate = new DateOnly(1969, 5, 1) },
            new ComponentManufacturer { Id = 2, Abbreviation = "NV", FullName = "NVIDIA Corporation", FoundationDate = new DateOnly(1993, 4, 5) },
            new ComponentManufacturer { Id = 3, Abbreviation = "COR", FullName = "Corsair Gaming Inc.", FoundationDate = new DateOnly(1994, 1, 1) }
        );

        modelBuilder.Entity<Component>().HasData(
            new Component { Code = "CPU0000001", Name = "Ryzen 7 7800X3D", Description = "8-core gaming processor", ComponentManufacturersId = 1, ComponentTypesId = 1 },
            new Component { Code = "GPU0000001", Name = "RTX 4080 Super", Description = "High-end gaming graphics card", ComponentManufacturersId = 2, ComponentTypesId = 2 },
            new Component { Code = "RAM0000001", Name = "Corsair Vengeance DDR5 16GB", Description = "DDR5 RAM module 16GB", ComponentManufacturersId = 3, ComponentTypesId = 3 }
        );

        modelBuilder.Entity<PC>().HasData(
            new PC { Id = 1, Name = "Gaming Beast X", Weight = 12.5, Warranty = 36, CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0), Stock = 5 },
            new PC { Id = 2, Name = "Office Mini Pro", Weight = 4.2, Warranty = 24, CreatedAt = new DateTime(2026, 4, 15, 13, 30, 0), Stock = 12 },
            new PC { Id = 3, Name = "Workstation Pro", Weight = 8.0, Warranty = 48, CreatedAt = new DateTime(2026, 3, 1, 10, 0, 0), Stock = 3 }
        );

        modelBuilder.Entity<PCComponent>().HasData(
            new PCComponent { PCId = 1, ComponentCode = "CPU0000001", Amount = 1 },
            new PCComponent { PCId = 1, ComponentCode = "GPU0000001", Amount = 1 },
            new PCComponent { PCId = 1, ComponentCode = "RAM0000001", Amount = 2 },
            new PCComponent { PCId = 2, ComponentCode = "CPU0000001", Amount = 1 },
            new PCComponent { PCId = 2, ComponentCode = "RAM0000001", Amount = 1 },
            new PCComponent { PCId = 3, ComponentCode = "CPU0000001", Amount = 2 },
            new PCComponent { PCId = 3, ComponentCode = "GPU0000001", Amount = 1 }
        );
    }
}
