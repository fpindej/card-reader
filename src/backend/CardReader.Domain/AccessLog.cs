namespace CardReader.Domain;

public class AccessLog
{
    public int Id { get; set; }
    public DateTime EventDateTime { get; set; }
    public string CardNumber { get; set; } = null!;
    public bool IsSuccessful { get; set; }
}
