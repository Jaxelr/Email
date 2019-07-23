﻿using System.Collections.Generic;
using EmailService.Entities.Models;

namespace EmailService.Entities.Operations
{
    public class PostEmailRequest
    {
        public string From { get; set; }
        public IEnumerable<string> To { get; set; }
        public IEnumerable<string> Cc { get; set; }
        public IEnumerable<string> Bcc { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public Attachment Attachment { get; set; }
    }
}
