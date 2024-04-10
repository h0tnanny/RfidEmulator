using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RfidEmulator.Domain.Entity;

namespace RfidEmulator.Repository;

public sealed class RepositoryContext : DbContext
{
    public DbSet<Reader> Readers { get; set; } = null!;
    public DbSet<ReaderConfig> ReaderConfigs { get; set; } = null!;
    public DbSet<Antenna> Antennas { get; set; } = null!;

    public RepositoryContext(DbContextOptions options) : base(options)
    {
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}