using RfidEmulator.Domain.Enum;

namespace RfidEmulator.Domain.DTOs;

public class ReaderDto
{
    public required string Ip { get; set; }
    public required int Port { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public TypeReader Type { get; set; }
    public required ReaderConfigDto Config { get; set; }
    public ICollection<AntennaDto>? Antennas { get; set; }
}