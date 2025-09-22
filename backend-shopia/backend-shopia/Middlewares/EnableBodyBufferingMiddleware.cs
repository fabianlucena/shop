namespace backend_shopia.Middlewares
{
    public class EnableBodyBufferingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            if (!request.HasFormContentType)
            {
                request.EnableBuffering();
                request.Body.Position = 0;
            }

            await next(context);
        }
    }
}
