using DatingApp.DAL.Context;
using DatingApp.DAL.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DatingApp.DAL.Extensions;

public static class WebApplicationExtension
{
    public static async Task ApplySeedDataInDataAccessLayer(this WebApplication app, ILogger logger)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<DataContext>();
            await context.Database.MigrateAsync();
            await Seed.SeedUsers(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occured during migration");
        }
    }
}