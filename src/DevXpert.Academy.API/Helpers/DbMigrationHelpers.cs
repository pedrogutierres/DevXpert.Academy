using DevXpert.Academy.Alunos.Data;
using DevXpert.Academy.API.Authentication;
using DevXpert.Academy.Conteudo.Data;
using DevXpert.Academy.Financeiro.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DevXpert.Academy.API.Helpers
{
    public static class DbMigrationHelperExtension
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            DbMigrationHelpers.EnsureSeedData(app).Wait();
        }
    }

    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var conteudoContext = scope.ServiceProvider.GetRequiredService<ConteudoContext>();
            var alunosContext = scope.ServiceProvider.GetRequiredService<AlunosContext>();
            var financeiroContext = scope.ServiceProvider.GetRequiredService<FinanceiroContext>();

            if (env.IsDevelopment())
            {
                await applicationDbContext.Database.MigrateAsync();
                await conteudoContext.Database.MigrateAsync();
                await alunosContext.Database.MigrateAsync();
                await financeiroContext.Database.MigrateAsync();

                await EnsureSeedProducts(scope, applicationDbContext);
            }
        }

        private static async Task EnsureSeedProducts(IServiceScope scope, ApplicationDbContext context)
        {
            if (await context.Users.AnyAsync())
                return;

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var user = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "admin@academy.com",
                NormalizedUserName = "ADMIN@ACADEMY.COM",
                Email = "ADMIN@ACADEMY.COM",
                NormalizedEmail = "ADMIN@ACADEMY.COM",
                AccessFailedCount = 0,
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(user, "Academy@123456");

            if (!result.Succeeded)
                return;

            if (!await roleManager.RoleExistsAsync("Administrador"))
            {
                var role = new IdentityRole();
                role.Name = "Administrador";
                await roleManager.CreateAsync(role);
            }

            await userManager.AddToRoleAsync(user, "Administrador");

            if (!await roleManager.RoleExistsAsync("Aluno"))
            {
                var role = new IdentityRole();
                role.Name = "Aluno";
                await roleManager.CreateAsync(role);
            }

            await context.SaveChangesAsync();
        }
    }
}
