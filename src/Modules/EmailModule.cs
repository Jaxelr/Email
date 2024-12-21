using Carter;
using Carter.OpenApi;
using Email.Extensions;
using Email.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Em = Email.Models;

namespace EmailService.Modules;

public class EmailModule : ICarterModule
{
    private const string ModuleTag = "Email";
    private const string ApplicationJson = "application/json";

    public void AddRoutes(IEndpointRouteBuilder app) =>
    app.MapPost("/Email", (HttpContext ctx, Em.Email email, IEmailRepository repository) =>
    {
        return ctx.ExecHandler(email, (request) =>
        {
            bool ack = repository.From(request.From)
                                .To(request.To!)
                                .Cc(request.Cc!)
                                .Bcc(request.Bcc!)
                                .Body(request.Body)
                                .Subject(request.Subject)
                                .Attach(request.Attachment)
                                .BodyAsHtml()
                                .SendAsync()
                                .Result;

            return new Em.EmailAck()
            {
                Successful = ack,
                Message = $"Message sent at: {System.DateTime.Now}"
            };
        });
    })
    .Produces<Em.EmailAck>(StatusCodes.Status200OK)
    .Produces<Em.FailedResponse>(StatusCodes.Status400BadRequest)
    .Produces<Em.FailedResponse>(StatusCodes.Status500InternalServerError)
    .WithName("PostEmail")
    .WithTags(ModuleTag)
    .Accepts<Em.Email>(ApplicationJson)
    .IncludeInOpenApi();
}
