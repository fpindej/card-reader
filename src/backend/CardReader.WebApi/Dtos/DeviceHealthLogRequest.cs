namespace CardReader.WebApi.Dtos;

public record DeviceHealthLogRequest(
    int DeviceId,
    int MaxAllocHeap,
    int MinFreeHeap,
    int FreeHeap,
    int Uptime,
    int FreeSketchSpace);
