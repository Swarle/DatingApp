using System.Text.Json;
using DatingApp.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utility;

namespace DatingApp.DAL.Infrastructure;

public class Seed
{
    public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        if (await userManager.Users.AnyAsync()) return;
        
        var userData = await
            File.ReadAllTextAsync("UserSeedData.json");
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
        
        if(users == null) return;

        var roles = new List<AppRole>
        {
            new AppRole { Name = "Member" },
            new AppRole { Name = "Admin" },
            new AppRole { Name = "Moderator" }
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        foreach (var user in users)
        {
            user.UserName = user.UserName!.ToLower();
            
            await userManager.CreateAsync(user, "Password1234");
            await userManager.AddToRoleAsync(user, RolesName.Member);
        }

        var admin = new AppUser
        {
            UserName = "admin",
            Gender = "male",
            KnownAs = "admin",
            City = "London",
            Country = "UK"
        };

        var result = await userManager.CreateAsync(admin, "Password1234");
        await userManager.AddToRolesAsync(admin, new[] {RolesName.Admin, RolesName.Moderator} );
    }
}