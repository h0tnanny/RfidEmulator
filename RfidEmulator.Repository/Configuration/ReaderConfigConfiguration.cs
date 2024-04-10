using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RfidEmulator.Domain.Entity;

namespace RfidEmulator.Repository.Configuration;

public class ReaderConfigConfiguration : IEntityTypeConfiguration<ReaderConfig>
{
    public void Configure(EntityTypeBuilder<ReaderConfig> builder)
    {
        builder.ToTable("ReaderConfigs", "equipment");
        builder.HasKey(key => key.Id);
        builder.HasIndex(index => index.Id);
    }
}