using DatingApp.DAL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.BL.Extensions;

public static class BusinessLogicLayerIdentityServicesExtensions
{
    public static IServiceCollection AddBusinessLogicLayerIdentityServices(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDataAccessLayerIdentityServices(config);

        return services;
    }
}