using Api.Model.Properties;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;
using ent = Api.Model.Email.Entities;

namespace Api.Model.Email.Operations
{
    [Api("Send Email")]
    [Route("/SendEmail", Verbs = "GET POST PUT", Summary = "Sends the email based on the request.", Notes = "Sends the email based on the request.")]
    [DataContract(Namespace = Default.Namespace)]
    public class SendEmail : IReturn<SendEmailResponse>
    {
        [DataMember]
        public ent.Email Email { get; set; }

        public SendEmail()
        {
            Email = new ent.Email
            {
                Attachment = new ent.Attachment()
            };
        }
    }

    [DataContract(Namespace = Default.Namespace)]
    public class SendEmailResponse : ent.BaseResponse
    {
        [DataMember]
        public bool SuccessfullyExecuted { get; set; }
    }
}
