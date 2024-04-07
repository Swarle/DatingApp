using API.Extensions;
using API.Infrastructure.Middlewares;
using DatingApp.BL.Extensions;
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
            
            builder.Services.AddBusinessLogicLayer(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "DatingApp",
                    Version = "v1"
                });
                
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token"
                });
                
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new string[] {}
                    }
                });
            });

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
