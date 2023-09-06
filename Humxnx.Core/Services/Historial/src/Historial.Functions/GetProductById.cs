using System;
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
        [FunctionName("GetProductById")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id}")] HttpRequest req,
            Guid id,
            ILogger log)
        {
            log.LogInformation("GetProductById HTTP trigger function invoked.");
            
            var product = new ProductoController();

            return new OkObjectResult(product.Get(id));
        }
    }
}
