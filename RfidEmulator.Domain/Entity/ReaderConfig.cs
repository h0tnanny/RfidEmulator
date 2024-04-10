namespace RfidEmulator.Domain.Entity;

public class ReaderConfig
{
    public Guid Id { get; set; }
    public int CountsPerSecTimeMin { get; set; }
    public int CountsPerSecTimeMax { get; set; }
    public int UpperRssiLevelMin { get; set; }
    public int UpperRssiLevelMax { get; set; }
    public int Tags { get; set; }
    
    public Reader? Reader { get; set; }
}