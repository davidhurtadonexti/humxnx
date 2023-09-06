using System;
using Humxnx.Historial.Core.Application.Interfaces;
using Humxnx.Historial.Core.Domain.Entities;
using Humxnx.Historial.Core.Infrastructure.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Humxnx.Historial.Core.Functions
{
    public static class GetProducts
    {
        private readonly IServicioBase<Producto,Guid> _productService;

        public static void CalculateCircleAreaFunction(IServicioBase<Producto,Guid> productService)
        {
            _productService = productService;
        }

        [FunctionName("GetProducts")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetProducts trigger function invoked.");
            var products = new ProductoController();
            // var products = Product.GetAllProducts();
            
            return new OkObjectResult(products.Get());
        }
    }
}