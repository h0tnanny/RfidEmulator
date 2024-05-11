namespace RfidEmulator.Domain.Entity;

public class OptimizationShortConfig
{
    public Guid ReaderId { get; set; }
    public int? CountsPerSec { get; set; }
}