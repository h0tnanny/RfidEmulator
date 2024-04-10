using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RfidEmulator.Domain.Entity;

namespace RfidEmulator.Repository.Configuration;

public class ReaderConfiguration : IEntityTypeConfiguration<Reader>
{
    public void Configure(EntityTypeBuilder<Reader> builder)
    {
        builder.ToTable("Readers", "equipment");
        builder.HasKey(key => key.Id);
        builder.HasIndex(index => index.Id);
    }
}