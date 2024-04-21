using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DatingApp.DAL.Context;
using DatingApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.DAL.Infrastructure;

public class Seed
{
    public static async Task SeedUsers(DataContext context)
    {
        if (await context.Users.AnyAsync()) return;
        
        var userData = await
            File.ReadAllTextAsync("UserSeedData.json");
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
        
        if(users == null) return;

        foreach (var user in users)
        {
            using var hmac = new HMACSHA512();

            user.UserName = user.UserName!.ToLower();


            await context.Users.AddAsync(user);
        }

        await context.SaveChangesAsync();
    }
}