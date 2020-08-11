using System;
using System.Threading.Tasks;
using Carter.ModelBinding;
using Carter.Response;
using Email.Models;
using Microsoft.AspNetCore.Http;

namespace EmailService.Extensions
{
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Encapsulate execution of handler with the corresponding validation logic
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="res">An http response that will be populated</param>
        /// <param name="handler">A func handler that will be validated and executed</param>
        /// <returns></returns>
        public static async Task ExecHandler<TOut>(this HttpResponse res, Func<TOut> handler)
        {
            try
            {
                var response = handler();

                if (response == null)
                {
                    res.StatusCode = 204;
                    return;
                }

                res.StatusCode = 200;
                await res.Negotiate(response);
            }
            catch (Exception ex)
            {
                res.StatusCode = 500;
                await res.Negotiate(new FailedResponse(ex));
            }
        }

        /// <summary>
        /// Encapsulate execution of handler with the validation logic while binding and validating the http request
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="res">An http response that will be populated</param>
        /// <param name="req">An http request that will be binded and validated</param>
        /// <param name="handler">A func handler that will be validated and executed</param>
        /// <returns></returns>
        public static async Task ExecHandler<TIn, TOut>(this HttpResponse res, HttpRequest req, Func<TIn, TOut> handler)
        {
            try
            {
                var (validationResult, data) = await req.BindAndValidate<TIn>();

                if (!validationResult.IsValid)
                {
                    res.StatusCode = 422;
                    await res.Negotiate(validationResult.GetFormattedErrors());
                    return;
                }

                var response = handler(data);

                if (response == null)
                {
                    res.StatusCode = 204;
                    return;
                }

                res.StatusCode = 200;
                await res.Negotiate(response);
            }
            catch (Exception ex)
            {
                res.StatusCode = 500;
                await res.Negotiate(new FailedResponse(ex));
            }
        }
    }
}
