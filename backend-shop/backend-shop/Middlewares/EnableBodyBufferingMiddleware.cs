namespace backend_shop.Middlewares
{
    public class EnableBodyBufferingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            request.EnableBuffering();
            request.Body.Position = 0;
            await next(context);
        }
    }
}
