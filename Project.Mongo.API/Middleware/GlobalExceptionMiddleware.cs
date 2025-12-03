using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Project.Mongo.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (ValidationException ex)
            {
                ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ctx.Response.ContentType = "application/json";
                var payload = new { message = "Validation failed", errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) };
                await ctx.Response.WriteAsync(JsonConvert.SerializeObject(payload));
            }
            catch (Exception ex)
            {
                ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ctx.Response.ContentType = "application/json";
                var payload = new { message = ex.Message };
                await ctx.Response.WriteAsync(JsonConvert.SerializeObject(payload));
            }
        }
    }
}
