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
    private const string ModuleTag = "Email";
    private const string ApplicationJson = "application/json";

    public void AddRoutes(IEndpointRouteBuilder app) =>
    app.MapPost("/Email", (HttpContext ctx, PostEmailRequest email, IEmailRepository repository) =>
    {
        return ctx.ExecHandler(email, (request) =>
        {
            bool ack = repository.From(request.From)
                                .To(request.To)
                                .Cc(request.Cc)
                                .Bcc(request.Bcc)
                                .Body(request.Body)
                                .Subject(request.Subject)
                                .Attach(request.Attachment)
                                .BodyAsHtml()
                                .SendAsync()
                                .Result;

            return new PostEmailResponse()
            {
                Successful = ack,
                Message = $"Message sent at: {System.DateTime.Now}"
            };
        });
    })
    .Produces<PostEmailResponse>(StatusCodes.Status200OK)
    .Produces<FailedResponse>(StatusCodes.Status400BadRequest)
    .Produces<FailedResponse>(StatusCodes.Status500InternalServerError)
    .WithName("PostEmail")
    .WithTags(ModuleTag)
    .Accepts<PostEmailRequest>(ApplicationJson)
    .IncludeInOpenApi();
}
