using a.Models;

using Microsoft.EntityFrameworkCore;

namespace a;

public class CamDataContext : DbContext
{
    public DbSet<Cam> Cams { get; set; }
    public DbSet<Temp> Temps { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<Active> Actives { get; set; }

    public CamDataContext()
    {
        
    }
    public CamDataContext(DbContextOptions<CamDataContext> options)
        : base(options)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=cam.db");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>()
            .HasMany(a => a.Cameras)
            .WithOne(c => c.Area)
            .HasForeignKey(c => c.AreaId)
            .IsRequired();

        modelBuilder.Entity<Area>()
            .HasMany(a => a.Actives)
            .WithOne(c => c.Area)
            .HasForeignKey(c => c.AreaId)
            .IsRequired();
    }
}


