using RFService.IExceptions;

namespace backend_ofertas
{
    public class ErrorMiddleware(RequestDelegate next, ILogger<ErrorMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch(Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            logger.LogError(exception, "An unexpected error occurred.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (exception as IHttpException)?.StatusCode ?? 500;
            await context.Response.WriteAsJsonAsync(new
            {
                Error = exception.GetType().Name,
                Message = exception.Message,
            });
        }
    }
}
