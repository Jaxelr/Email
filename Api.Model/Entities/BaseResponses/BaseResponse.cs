using Api.Model.Properties;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;

namespace Api.Model.Email.Entities
{
    [DataContract(Namespace = Default.Namespace)]
    public class BaseResponse
    {
        [DataMember(Order = 1)]
        public int? Records { get; set; }

        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }

        [DataMember(Order = 3)]
        public long ElapsedMiliseconds { get; set; }
    }
}