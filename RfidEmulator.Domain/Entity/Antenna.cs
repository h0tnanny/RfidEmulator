namespace RfidEmulator.Domain.Entity;

public class Antenna
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public int? Power { get; set; }
}