using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
namespace Humxnx.Historial.Core.Functions
{
    public static class UpdateProduct
    {
        [FunctionName("UpdateProduct")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "products/{id}")] HttpRequest req,
            int id,
            ILogger log)
        {
            log.LogInformation("UpdateProduct HTTP trigger function invoked.");
            return new NoContentResult();
        }
    }
}
