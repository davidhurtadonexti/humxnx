using System;
using Humxnx.Historial.Core.Application.Interfaces;
using Humxnx.Historial.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Humxnx.Historial.Core.Infrastructure.Controllers;
namespace Humxnx.Historial.Core.Functions
{
    public static class GetProductById
    {
        
         private static IServicioBase<Producto,Guid> _productService;

        public static void ProductoCntroller(IServicioBase<Producto,Guid> productService)
        {
            _productService = productService;
        }
        
        [FunctionName("GetProductById")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id}")] HttpRequest req,
            Guid id,
            ILogger log)
        {
            log.LogInformation("GetProductById HTTP trigger function invoked.");
            

            return new OkObjectResult(_productService.SeleccionarPorID(id));
        }
    }
}
