namespace RfidEmulator.Domain.Entity;

public class OptimizationConfig
{
    public Guid ReaderId { get; set; }
    public int? CountsPerSecTimeMin { get; set; }
    public int? CountsPerSecTimeMax { get; set; }
    public int? UpperRssiLevelMin { get; set; }
    public int? UpperRssiLevelMax { get; set; }
    public int? Tags { get; set; }
}