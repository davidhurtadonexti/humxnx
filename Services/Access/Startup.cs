using System;
using Access;
using Access.Auth.Domain.Interfaces;
using Access.Auth.Infrastructure.Controllers;
using Access.Auth.Infrastructure.Repository;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Access
{
    internal class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<LoginController>();
            builder.Services.AddTransient<MenusController>();
            builder.Services.AddTransient<EnterprisesController>();
            builder.Services.AddTransient<ModulesController>();
            builder.Services.AddTransient<ProfilesController>();
            builder.Services.AddTransient<ProtectedDataController>();
            builder.Services.AddTransient<ResourcesController>();
            builder.Services.AddTransient<TokensController>();
            builder.Services.AddTransient<UsersController>();
            // builder.Services.AddSingleton<IConnection, Connection>();
            builder.Services.AddSingleton<IEnterprises, EnterprisesRepository>();
            builder.Services.AddSingleton<ILogin, LoginRepository>();
            builder.Services.AddSingleton<IMenus, MenusRepository>();
            builder.Services.AddSingleton<IModules, ModulesRepository>();
            builder.Services.AddSingleton<IProfiles, ProfilesRepository>();
            builder.Services.AddSingleton<IProtectedData, ProtectedDataRepository>();
            builder.Services.AddSingleton<IResources, ResourcesRepository>();
            builder.Services.AddSingleton<ITokens, TokensRepository>();
            builder.Services.AddSingleton<IUsers, UsersRepository>();
            builder.Services.AddScoped<IEnterprises, EnterprisesRepository>();
            builder.Services.AddScoped<ILogin, LoginRepository>();
            builder.Services.AddScoped<IMenus, MenusRepository>();
            builder.Services.AddScoped<IModules, ModulesRepository>();
            builder.Services.AddScoped<IProfiles, ProfilesRepository>();
            builder.Services.AddScoped<IProtectedData, ProtectedDataRepository>();
            builder.Services.AddScoped<IResources, ResourcesRepository>();
            builder.Services.AddScoped<ITokens, TokensRepository>();
            builder.Services.AddScoped<IUsers, UsersRepository>();
            builder.Services.AddSingleton<IConfiguration>(config =>
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();

                return builder.Build();
            });
        }
    }
}