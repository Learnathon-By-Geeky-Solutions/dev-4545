using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Employee.API.ExceptionHandler
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var response = new
            {
                StatusCode=httpContext.Response.StatusCode,
                Message=exception.Message,
                Title="Something went wrong."
            };

            

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return false;
        }
    }
}
