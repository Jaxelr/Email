using Carter;
using EmailService.Cache;
using EmailService.Entities.Operations;
using EmailService.Extensions;
using EmailService.Modules.Metadata;
using EmailService.Repositories;

namespace EmailService.Modules
{
    public class EmailModule : CarterModule
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly IEmailRepository repository;
        private readonly Store store;
#pragma warning restore IDE0052 // Remove unread private members

        public EmailModule(IEmailRepository repository, Store store)
        {
            this.repository = repository;
            this.store = store;

            Post<PostEmail>("/Email", (req, res, routeData) =>
            {
                return res.ExecHandler<PostEmailRequest, PostEmailResponse>(req, (request) =>
                {
                    bool successfullyExecuted = repository.From(request.From)
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
                        Successful = successfullyExecuted,
                        Message = $"Message sent at: {System.DateTime.Now}"
                    };
                });
            });
        }
    }
}
