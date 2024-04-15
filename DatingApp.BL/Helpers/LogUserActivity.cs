using DatingApp.BL.Extensions;
using DatingApp.DAL.Entities;
using DatingApp.DAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Utility;

namespace DatingApp.BL.Helpers;

public class LogUserActivity : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();
        
        if(!resultContext.HttpContext.User.Identity!.IsAuthenticated) return;

        var userId = resultContext.HttpContext.User.GetUserId() ??
                     throw new InvalidOperationException(SD.InvalidOperationMessage);

        var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IRepository<AppUser>>();
        
        var user = await repo.GetByIdAsync(userId) ??
                   throw new InvalidOperationException(SD.InvalidOperationMessage);
        
        user.LastActive = DateTime.UtcNow;
        await repo.SaveChangesAsync();
    }
}