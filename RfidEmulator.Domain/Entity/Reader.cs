using RfidEmulator.Domain.Enum;

namespace RfidEmulator.Domain.Entity;

public class Reader
{
     public Guid Id { get; set; }
     public required string Ip { get; set; }
     public required int Port { get; set; }
     public string? Name { get; set; }
     public string? Description { get; set; }
     public TypeReader Type { get; set; }
     public required ReaderConfig Config { get; set; }
     public Guid ReaderConfigId { get; set; }
     public ICollection<Antenna>? Antennas { get; set; }
}