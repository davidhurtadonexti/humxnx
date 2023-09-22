using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OAuth2.src.OAuth2.Infraestructure.Contracts.BindToken;
using OAuth2.src.OAuth2.Infraestructure.Controllers;
using System.IO;
using System.Threading.Tasks;

namespace Humxnx.OAuth2.Core.Functions
{
    public class BindTokenFunction
    {
        private readonly BindTokenController _method;

        public BindTokenFunction(BindTokenController method )
        {
            _method = method;
        }

        [FunctionName("BindToken")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "bindToken")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("BindToken trigger function invoked.");
            
            var requestBody = new StreamReader(req.Body).ReadToEnd();

            return await _method.Post(JsonConvert.DeserializeObject<BindTokenInput>(requestBody), req);
        }
    }
}