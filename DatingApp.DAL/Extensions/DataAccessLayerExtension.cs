using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatingApp.DAL.Context;
using DatingApp.DAL.Entities;
using DatingApp.DAL.Repository;
using DatingApp.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.DAL.Extensions
{
    public static class DataAccessLayerExtension
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IRepository<AppUser>, Repository<AppUser>>();

            return services;
        }

    }
}
