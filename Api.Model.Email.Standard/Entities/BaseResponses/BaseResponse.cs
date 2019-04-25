using Api.Model.Properties;
using System.Runtime.Serialization;

namespace Api.Model.Email.Entities
{
    public class BaseResponse
    {
        public int? Records { get; set; }

        public ResponseStatus ResponseStatus { get; set; }

        public long ElapsedMiliseconds { get; set; }
    }
}
