using System.Collections.Generic;

namespace EmailService.Models.Operations
{
    public class PostEmailRequest
    {
        public string From { get; set; }
        public ICollection<string> To { get; set; }
        public ICollection<string> Cc { get; set; }
        public ICollection<string> Bcc { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public Attachment Attachment { get; set; }
    }
}
