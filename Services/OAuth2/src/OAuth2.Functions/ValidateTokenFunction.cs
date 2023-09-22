using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OAuth2.src.OAuth2.Infraestructure.Contracts.ValidateToken;
using OAuth2.src.OAuth2.Infraestructure.Controllers;
using System.IO;
using System.Threading.Tasks;

namespace Humxnx.OAuth2.Core.Functions
{
    public class ValidateTokenFunction
    {
        private readonly ValidateTokenController _method;

        public ValidateTokenFunction(ValidateTokenController method )
        {
            _method = method;
        }

        [FunctionName("ValidationToken")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "validateToken")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("ValidationToken trigger function invoked.");
            
            var requestBody = new StreamReader(req.Body).ReadToEnd();

            return await _method.Post(JsonConvert.DeserializeObject<ValidateTokenInput>(requestBody), req);
        }
    }
}