using System.Collections.Generic;

namespace Email.Models.Operations;

public class PostEmailRequest
{
    public string From { get; set; } = string.Empty;
    public ICollection<string>? To { get; set; }
    public ICollection<string>? Cc { get; set; }
    public ICollection<string>? Bcc { get; set; }
    public string Body { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public Attachment Attachment { get; set; } = new();
}
