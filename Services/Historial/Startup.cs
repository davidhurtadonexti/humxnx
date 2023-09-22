
using Historial;
using Humxnx.Historial.Core.Application.Interfaces;
using Humxnx.Historial.Core.Application.Services;
using Humxnx.Historial.Core.Domain.Entities;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;


[assembly: FunctionsStartup(typeof(Startup))]

namespace Historial
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = BuildConfiguration(builder.GetContext().ApplicationRootPath);
            builder.Services.Configure<IConfiguration>(configuration);
            builder.Services.AddScoped<IServicioBase<Producto,Guid>, ProductoServicio>();
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