using ent = Api.Model.Email.Entities;

namespace Api.Model.Email.Operations
{
    public class SendEmail
    {
        public ent.Email Email { get; set; }

        public SendEmail()
        {
            Email = new ent.Email
            {
                Attachment = new ent.Attachment()
            };
        }
    }

    public class SendEmailResponse : ent.BaseResponse
    {
        public bool SuccessfullyExecuted { get; set; }
    }
}
