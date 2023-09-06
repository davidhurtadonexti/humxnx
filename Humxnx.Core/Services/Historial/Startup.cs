using System;
using Humxnx.Historial.Core.Application.Interfaces;
using Humxnx.Historial.Core.Application.Services;
using Humxnx.Historial.Core.Domain.Entities;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

// [assembly: FunctionsStartup(typeof(Humxnx.Startup))]

namespace Humxnx.Historial
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Aquí es donde registrarás las extensiones necesarias.
            // Por ejemplo, para Azure Storage:
            // builder.Services.AddAzureStorage();
            // builder.Services.AddServiceBus();
             builder.Services.AddScoped<IServicioBase<Producto,Guid>, ProductoServicio>();
        }
    }
}