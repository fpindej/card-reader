namespace CardReader.WebApi.Dtos;

public class AccessLogRequest
{
    public string CardNumber { get; set; } = null!;
    public bool IsSuccessful { get; set; }
    public DateTime Timestamp { get; set; }
}
