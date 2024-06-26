﻿using DatingApp.BL.Helpers;
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

            services.AddScoped<LogUserActivity>();

            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IAdminService, AdminService>();
                
            return services;
        }
    }
}
