using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            return services;
        }
    }
}
