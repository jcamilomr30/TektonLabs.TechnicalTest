using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace TektonLabs.TechnicalTest.Infraestructure.Exceptions
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger _logger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                await _next(context);
            }
            catch (RequestValidationException rve)
            {
                ErrorLog(rve);
                await HandleExceptionAsync(context, rve, (HttpStatusCode)rve.Details.Status);
            }
            catch (BusinessValidationException bve)
            {
                ErrorLog(bve);
                await HandleExceptionAsync(context, bve, (HttpStatusCode)bve.Details.Status);
            }
            catch (NotFoundException nfe)
            {
                ErrorLog(nfe);
                await HandleExceptionAsync(context, nfe, nfe.Status);
            }
            catch (Exception ex)
            {
                ErrorLog(ex);
                var statusCode = HttpStatusCode.InternalServerError;
                if (ex is HttpException)
                {
                    statusCode = ((HttpException)ex).HttpStatusCode;
                }
                var httpException = new HttpException(ex.Message, ex, HttpStatusCode.InternalServerError);

                await HandleExceptionAsync(context, httpException, statusCode);
            }
            finally
            {
                var controllerName = context.GetRouteData().Values["controller"];
                var actionName = context.GetRouteData().Values["action"];

                stopwatch.Stop();
                var responseTime = stopwatch.ElapsedMilliseconds;
                _logger.Information($"API: {controllerName}/{actionName} Time: {responseTime} ms");
            }
        }

        private void ErrorLog(Exception ex)
        {
            _logger.Error(ex, null);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode httpCode = HttpStatusCode.InternalServerError)
        {
            var id = Guid.NewGuid();
            _logger.Error(exception, $"{id}: {exception.Message}");
            var result = JsonConvert.SerializeObject(new { error = exception.ToString(), id });
            if (exception is RequestValidationException)
            {
                var requestValidationException = exception as RequestValidationException;
                result = JsonConvert.SerializeObject(new { error = requestValidationException.Details.Errors, id });
            }
            if (exception is BusinessValidationException)
            {
                var requestValidationException = exception as BusinessValidationException;
                result = JsonConvert.SerializeObject(new { error = requestValidationException.Details.Errors, id });
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpCode;
            return context.Response.WriteAsync(result);
        }

        public class HttpException : Exception
        {
            public HttpException(string message, HttpStatusCode httpStatusCode) : base(message)
            {
                HttpStatusCode = httpStatusCode;
            }

            public HttpException(string message, Exception exception, HttpStatusCode httpStatusCode) : base(message, exception)
            {
                HttpStatusCode = httpStatusCode;
            }

            public HttpStatusCode HttpStatusCode { get; private set; }
        }
    }
}
