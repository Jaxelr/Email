using System;
using System.Threading.Tasks;
using Carter.ModelBinding;
using Carter.Response;
using Email.Models;
using Microsoft.AspNetCore.Http;

namespace Email.Extensions;

public static class HttpContextExtensions
{
    /// <summary>
    /// Encapsulate execution of handler with the corresponding validation logic
    /// </summary>
    /// <typeparam name="TOut">Type of the response</typeparam>
    /// <param name="ctx">The http context to process</param>
    /// <param name="handler">A func handler that will be validated and executed</param>
    /// <returns>A Task</returns>
    public static async Task ExecHandler<TOut>(this HttpContext ctx, Func<TOut> handler)
    {
        try
        {
            var response = handler();

            if (response is null)
            {
                ctx.Response.StatusCode = StatusCodes.Status204NoContent;
                return;
            }

            await ctx.NegotiateResponse(response, StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            await ctx.NegotiateResponse(new FailedResponse(ex), StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Encapsulate execution of handler with the validation logic while binding and validating the http request
    /// </summary>
    /// <typeparam name="TIn">Type of the request</typeparam>
    /// <typeparam name="TOut">Type of the response</typeparam>
    /// <param name="ctx">The http context to process</param>
    /// <param name="in">The instance of the type for request</param>
    /// <param name="handler">A func handler that will be validated and executed</param>
    /// <returns>A Task</returns>
    public static async Task ExecHandler<TIn, TOut>(this HttpContext ctx, TIn @in, Func<TIn, TOut> handler)
    {
        try
        {
            var result = ctx.Request.Validate(@in);

            if (!result.IsValid)
            {
                await ctx.NegotiateResponse(result.GetFormattedErrors(), StatusCodes.Status422UnprocessableEntity);
                return;
            }

            var response = handler(@in);

            if (response is null)
            {
                ctx.Response.StatusCode = StatusCodes.Status204NoContent;
                return;
            }

            await ctx.NegotiateResponse(response, StatusCodes.Status200OK);
        }
        catch (ArgumentNullException ex)
        {
            await ctx.NegotiateResponse(new FailedResponse(ex), StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            await ctx.NegotiateResponse(new FailedResponse(ex), StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task NegotiateResponse<T>(this HttpContext ctx, T response, int statusCode)
    {
        ctx.Response.StatusCode = statusCode;
        await ctx.Response.Negotiate(response);
    }
}
