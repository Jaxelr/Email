using Api.Model.Properties;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Api.Model.Email.Entities
{
    [DataContract(Namespace = Default.Namespace)]
    public class Email
    {
        [DataMember(Order = 1)]
        public string From { get; set; }

        [DataMember(Order = 2)]
        public IEnumerable<string> To { get; set; }

        [DataMember(Order = 3)]
        public IEnumerable<string> Cc { get; set; }

        [DataMember(Order = 4)]
        public IEnumerable<string> Bcc { get; set; }

        [DataMember(Order = 5)]
        public string Body { get; set; }

        [DataMember(Order = 6)]
        public string Subject { get; set; }

        [DataMember(Order = 7)]
        public Attachment Attachment { get; set; }
    }
}