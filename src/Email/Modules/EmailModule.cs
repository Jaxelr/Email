using Carter;
using Carter.OpenApi;
using Email.Extensions;
using Email.Models;
using Email.Models.Operations;
using Email.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EmailService.Modules;

public class EmailModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
    app.MapPost("/Email", (HttpRequest req, HttpResponse res, IEmailRepository repository) =>
    {
        return "PlaceHolder";
        //return res.ExecHandler<PostEmailRequest, PostEmailResponse>(req, (request) =>
        //{
        //    bool ack = repository.From(request.From)
        //                                    .To(request.To)
        //                                    .Cc(request.Cc)
        //                                    .Bcc(request.Bcc)
        //                                    .Body(request.Body)
        //                                    .Subject(request.Subject)
        //                                    .Attach(request.Attachment)
        //                                    .BodyAsHtml()
        //                                    .Send();

        //    return new PostEmailResponse()
        //    {
        //        Successful = ack,
        //        Message = $"Message sent at: {System.DateTime.Now}"
        //    };
        //});
    })
    .Produces<PostEmailResponse>(200)
    .Produces<FailedResponse>(500)
    .WithName("PostEmail")
    .WithTags("Email")
    .Accepts<PostEmailRequest>("application/json")
    .IncludeInOpenApi();
}
