namespace CardReader.Domain;

public class DeviceHealth
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public int MaxAllocHeap { get; set; }
    public int MinFreeHeap { get; set; }
    public int FreeHeap { get; set; }
    public int Uptime { get; set; }
    public int FreeSketchSpace { get; set; }
    public DateTime CreatedAt { get; set; }
}
