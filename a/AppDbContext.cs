using a.Models;

using Microsoft.EntityFrameworkCore;

namespace a;

public class AppDbContext : DbContext
{
    public DbSet<Temp> Temps { get; set; }

    public AppDbContext()
    { }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=temps.db");
        base.OnConfiguring(optionsBuilder);
    }
}
