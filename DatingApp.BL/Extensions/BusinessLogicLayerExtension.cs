using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatingApp.BL.Infrastructure;
using DatingApp.BL.Services;
using DatingApp.BL.Services.Interfaces;
using DatingApp.DAL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.BL.Extensions
{
    public static class BusinessLogicLayerExtension
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDataAccessLayer(configuration);
            
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
                
            return services;
        }
    }
}
