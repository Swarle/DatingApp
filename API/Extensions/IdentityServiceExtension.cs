using System.Text;
using DatingApp.BL.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Utility;

namespace API.Extensions
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBusinessLogicLayerIdentityServices(configuration);
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[SD.TokenKey]!)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(PoliciesName.RequireAdminRole, policy => policy.RequireRole(RolesName.Admin));
                opt.AddPolicy(PoliciesName.ModeratePhotoRole,
                    policy => policy.RequireRole(RolesName.Admin, RolesName.Moderator));
            });

            return services;
        }
    }
}
