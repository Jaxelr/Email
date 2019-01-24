using Api.Model.Email.Operations;
using Repoes;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceInterface;

namespace Api.ServiceInterface
{
    public class EmailService : Service
    {
        private IEmailRepository _repo;

        public EmailService(IEmailRepository repo)
        {
            _repo = repo;
        }

        public object Any(SendEmail request)
        {
            try
            {
                bool successfullyExecuted = _repo.From(request.Email.From)
                    .To(request.Email.To)
                    .Cc(request.Email.Cc)
                    .Bcc(request.Email.Bcc)
                    .Body(request.Email.Body)
                    .Subject(request.Email.Subject)
                    .Attach(request.Email.Attachment)
                    .BodyAsHtml()
                    .Send();

                return new SendEmailResponse() { SuccessfullyExecuted = successfullyExecuted };
            }
            catch (WebServiceException e)
            {
                throw e;
            }
        }
    }
}
