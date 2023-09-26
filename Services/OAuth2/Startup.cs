using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OAuth2;
using OAuth2.src.OAuth2.Application.Interfaces;
using OAuth2.src.OAuth2.Application.Services;
using OAuth2.src.OAuth2.Domain.Interfaces;
using OAuth2.src.OAuth2.Infraestructure.Controllers;
using OAuth2.src.OAuth2.Infraestructure.Repositories.DB;

[assembly: FunctionsStartup(typeof(Startup))]
namespace OAuth2
{
    internal class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<AuthorizationTokenController>();
            builder.Services.AddTransient<ValidateTokenController>();
            builder.Services.AddTransient<RefreshTokenController>();
            builder.Services.AddTransient<BindTokenController>();
            builder.Services.AddSingleton<IConnection, Connection>();
            builder.Services.AddSingleton<ITokenGenerator, TokenGeneratorService>();
            builder.Services.AddSingleton<IBindToken, BindTokenService>();
            builder.Services.AddSingleton<IValidateToken, ValidateTokenService>();
            builder.Services.AddSingleton<IRefreshToken, RefreshTokenService>();
            builder.Services.AddSingleton<IAuthorizationToken, AuthorizationTokenService>();
            builder.Services.AddScoped<IValidateToken, ValidateTokenService>();
            builder.Services.AddScoped<ITokenGenerator, TokenGeneratorService>();
            builder.Services.AddScoped<IBindToken, BindTokenService>();
            builder.Services.AddScoped<IValidateToken, ValidateTokenService>();
            builder.Services.AddScoped<IRefreshToken, RefreshTokenService>();
            builder.Services.AddScoped<IAuthorizationToken, AuthorizationTokenService>();
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