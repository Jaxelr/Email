namespace Email.Models;

public class EmailAck
{
    public bool Successful { get; set; }
    public string Message { get; set; } = string.Empty;
}
