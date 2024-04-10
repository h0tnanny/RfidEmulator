using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RfidEmulator.Domain.Entity;

namespace RfidEmulator.Repository.Configuration;

public class AntennaConfiguration : IEntityTypeConfiguration<Antenna>
{
    public void Configure(EntityTypeBuilder<Antenna> builder)
    {
        builder.ToTable("Antennas", "equipment");
        builder.HasKey(key => key.Id);
        builder.HasIndex(index => index.Id);
    }
}