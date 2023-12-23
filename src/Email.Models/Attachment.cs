namespace Email.Models;

public class Attachment
{
    public string ContentType { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public byte[]? Content { get; set; }
}
