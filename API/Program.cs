using API.Extensions;
using API.Infrastructure.Middlewares;
using DatingApp.BL.Extensions;
using DatingApp.BL.Infrastructure;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();

            builder.Services.AddCors();
            
            builder.Services.AddHttpContextAccessor();
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection(""));
            
            builder.Services.AddBusinessLogicLayer(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerWithSettings();

            var app = builder.Build();

            app.UseCors(policyBuilder =>
                policyBuilder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:4200")
            );

            app.UseApiExceptionMiddleware();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            await app.ApplySeedData(logger);
            
            
            
            await app.RunAsync();
        }
    }
}
