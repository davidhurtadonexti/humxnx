using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OAuth2.src.OAuth2.Infraestructure.Contracts.RefreshToken;
using OAuth2.src.OAuth2.Infraestructure.Controllers;
using System.IO;
using System.Threading.Tasks;

namespace Humxnx.OAuth2.Core.Functions
{
    public class RefreshTokenFunction
    {
        private readonly RefreshTokenController _method;

        public RefreshTokenFunction(RefreshTokenController method )
        {
            _method = method;
        }

        [FunctionName("RefreshToken")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "refreshToken")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("RefreshToken trigger function invoked.");
            
            var requestBody = new StreamReader(req.Body).ReadToEnd();

            return await _method.Post(JsonConvert.DeserializeObject<RefreshTokenInput>(requestBody), req);
        }
    }
}