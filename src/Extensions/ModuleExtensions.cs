using System;
using System.Threading.Tasks;
using Carter.ModelBinding;
using Carter.Response;
using Microsoft.AspNetCore.Http;

namespace EmailService.Extensions
{
    public static class ModuleExtensions
    {
        public static Task ExecHandler<TOut>(this HttpResponse res, Func<TOut> handler)
        {
            try
            {
                var response = handler();

                if (response == null)
                {
                    res.StatusCode = 204;
                    return Task.CompletedTask;
                }

                res.StatusCode = 200;
                return res.Negotiate(response);
            }
            catch (Exception ex)
            {
                res.StatusCode = 500;
                return res.Negotiate(ex.Message);
            }
        }

        public static Task ExecHandler<TIn, TOut>(this HttpResponse res, HttpRequest req, Func<TIn, TOut> handler)
        {
            try
            {
                var (validationResult, data) = req.BindAndValidate<TIn>();

                if (!validationResult.IsValid)
                {
                    res.StatusCode = 422;
                    return res.Negotiate(validationResult.GetFormattedErrors());
                }

                var response = handler(data);

                if (response == null)
                {
                    res.StatusCode = 204;
                    return Task.CompletedTask;
                }

                res.StatusCode = 200;
                return res.Negotiate(response);
            }
            catch (Exception ex)
            {
                res.StatusCode = 500;
                return res.Negotiate(ex.Message);
            }
        }
    }
}
