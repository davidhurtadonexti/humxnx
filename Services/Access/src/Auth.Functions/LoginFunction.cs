using System.IO;
using System.Threading.Tasks;
using Access.Auth.Infrastructure.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Access.Auth.Functions
{
    public class LoginFunction
    {
        private readonly LoginController _method;

        public LoginFunction(LoginController method)
        {
            _method = method;
        }
        
        [FunctionName("Login")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            // var requestBody = new StreamReader(req.Body).ReadToEnd();
      
            return  _method.LoadGrants();

            // return new OkObjectResult(responseMessage);
        }
    }
}
