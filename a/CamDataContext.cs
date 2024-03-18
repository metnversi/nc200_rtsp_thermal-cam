using a.Models;

using Microsoft.EntityFrameworkCore;

namespace a;

public class CamDataContext : DbContext
{
    public DbSet<Cam> Cams { get; set; }
    public DbSet<Temp> Temps { get; set; }

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
}


