using DatingApp.DAL.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace DatingApp.BL.Extensions;

public static class WebApplicationExtension
{
    public static async Task ApplySeedData(this WebApplication app, ILogger logger)
    {
        await app.ApplySeedDataInDataAccessLayer(logger);
    }
}