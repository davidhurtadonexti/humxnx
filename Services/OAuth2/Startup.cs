
using Historial;
// using Humxnx.OAuth2.Application.Interfaces;
// using Humxnx.OAuth2.Application.Services;
// using Humxnx.OAuth2.Domain.Entities;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using OAuth2.src.OAuth2.Application.Interfaces;
using OAuth2.src.OAuth2.Application.Services;


[assembly: FunctionsStartup(typeof(Startup))]

namespace Historial
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = BuildConfiguration(builder.GetContext().ApplicationRootPath);
            builder.Services.Configure<IConfiguration>(configuration);
            builder.Services.AddScoped<IValidateToken, ValidateTokenService>();
        }

        private IConfiguration BuildConfiguration(string applicationRootPath)
        {
            var config =
                new ConfigurationBuilder()
                    .SetBasePath(applicationRootPath)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

            return config;
        }
    }
}