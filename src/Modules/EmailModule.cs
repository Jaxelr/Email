using Carter;
using EmailService.Extensions;
using EmailService.Models.Operations;
using EmailService.Modules.Metadata;
using EmailService.Repositories;

namespace EmailService.Modules
{
    public class EmailModule : CarterModule
    {
        public EmailModule(IEmailRepository repository)
        {
            Post<PostEmail>("/Email", (req, res) =>
            {
                return res.ExecHandler<PostEmailRequest, PostEmailResponse>(req, (request) =>
                {
                    bool ack = repository.From(request.From)
                                                    .To(request.To)
                                                    .Cc(request.Cc)
                                                    .Bcc(request.Bcc)
                                                    .Body(request.Body)
                                                    .Subject(request.Subject)
                                                    .Attach(request.Attachment)
                                                    .BodyAsHtml()
                                                    .Send();

                    return new PostEmailResponse()
                    {
                        Successful = ack,
                        Message = $"Message sent at: {System.DateTime.Now}"
                    };
                });
            });
        }
    }
}
