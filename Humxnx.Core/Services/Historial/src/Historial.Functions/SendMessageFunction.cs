using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Humxnx.Historial.Core.Functions;

public static class SendMessageFunction
{


    [FunctionName("SendMessageFunction")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "send-message")] HttpRequest req,
        ILogger logger)
    {
        string connectionString = Environment.GetEnvironmentVariable("AzureServiceBusConnectionString");
        string queueName = "success_order_queue"; 
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        await using var client = new ServiceBusClient(connectionString);

        var sender = client.CreateSender(queueName);

        try
        {
            var message = new ServiceBusMessage(requestBody);
            await sender.SendMessageAsync(message);
            logger.LogInformation( "Mensaje enviado con éxito.");
            return new OkObjectResult($"Mensaje enviado");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Ocurrió una excepción en SendMessageFunction: {ex.Message}");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
        
        
    }
}
