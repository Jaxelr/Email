using Api.Model.Properties;
using System.Runtime.Serialization;

namespace Api.Model.Email.Entities
{
    [DataContract(Namespace = Default.Namespace)]
    public class Attachment
    {
        [DataMember(Order = 1)]
        public string ContentType { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public byte[] Content { get; set; }
    }
}