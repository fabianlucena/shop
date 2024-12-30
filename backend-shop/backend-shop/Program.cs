using Microsoft.OpenApi.Models;
using RFAuth;
using RFHttpExceptions.Middlewares;
using RFHttpExceptionsLocale.Middlewares;

namespace backend_shop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

            // Add services to the container.
            builder.ConfigureServices();

            //builder.Services.AddRouting(options => options.LowercaseUrls = true);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.ConfigureRepo();
            app.ConfigureData();

            app.UsePathBase("/api");
            app.UseRouting();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseCors("allowAll");
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<AuthorizationMiddleware>();
            app.UseMiddleware<HttpExceptionLocaleMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
