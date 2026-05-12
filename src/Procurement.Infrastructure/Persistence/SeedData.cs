using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Procurement.Domain.Entities;

namespace Procurement.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();

        await SeedRolesAsync(context);
        await SeedUsersAsync(context);
    }

    private static async Task SeedRolesAsync(ApplicationDbContext context)
    {
        var roleNames = new[] { "Staff", "Manager", "Admin" };

        foreach (var roleName in roleNames)
        {
            if (!await context.Roles.AnyAsync(r => r.Name == roleName))
            {
                context.Roles.Add(new Role { Name = roleName });
            }
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedUsersAsync(ApplicationDbContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
        if (adminRole is null)
            return;

        context.Users.Add(new User
        {
            Name = "Admin User",
            Email = "admin@procurement.local",
            RoleId = adminRole.Id
        });

        await context.SaveChangesAsync();
    }
}
