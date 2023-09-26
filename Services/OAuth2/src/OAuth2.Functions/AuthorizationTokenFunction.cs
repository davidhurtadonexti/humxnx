using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OAuth2.src.OAuth2.Infraestructure.Contracts.GetToken;
using OAuth2.src.OAuth2.Infraestructure.Controllers;
using System.IO;
using System.Threading.Tasks;

namespace Humxnx.OAuth2.Core.Functions
{
    public class AuthorizationTokenFunction
    {
        private readonly AuthorizationTokenController _method;

        public AuthorizationTokenFunction(AuthorizationTokenController method)
        {
            _method = method;
        }

        [FunctionName("AuthorizationToken")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "authorization-token")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("AuthorizationToken trigger function invoked.");

            
            var requestBody = new StreamReader(req.Body).ReadToEnd();
            var requestBodyString = JsonConvert.DeserializeObject<GetTokenInput>(requestBody);
      
            return await _method.Post(requestBodyString);
        }
    }
}