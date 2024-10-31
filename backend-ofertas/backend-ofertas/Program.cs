using Microsoft.OpenApi.Models;
using RFAuth;

namespace backend_ofertas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.Local.json");

            // Add services to the container.
            builder.ConfigureServices();

            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API Ofertas",
                    Description = "Aplicación para publicación y búsqueda de ofertas",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Fabian Lucena",
                        Email = "fabianlucena@gmail.com",
                        //Url = new Uri("https://socialNet.com/<user>"),
                    },
                    /*License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }*/
                });
            });

            var app = builder.Build();

            app.ConfigureRepo();
            app.ConfigureData();

            app.UsePathBase("/api");
            app.UseRouting();
            app.UseCors("allowAll");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(/*options => // UseSwaggerUI is called only in Development.
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                }*/);
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<AuthorizationMiddleware>();
            app.UseMiddleware<ErrorMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
