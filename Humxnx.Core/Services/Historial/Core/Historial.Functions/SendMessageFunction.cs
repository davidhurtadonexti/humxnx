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
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        string connectionString = configuration.GetConnectionString("AzureServiceBusConnectionString");
        string queueName = "success_order_queue"; // Reemplaza con el nombre de tu cola
        logger.LogInformation($"C# HTTP trigger function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        await using var client = new ServiceBusClient("Endpoint=sb://humana-queue-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=t+5hbC+q9libg/wYqukzSH5iN7MtdgGzN+ASbCgDM78=");
        var sender = client.CreateSender(queueName);

        try
        {
            var message = new ServiceBusMessage(requestBody);
            await sender.SendMessageAsync(message);
            logger.LogInformation( "Mensaje enviado con éxito.");
            return new OkObjectResult($"Mensaje enviado: {message}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Ocurrió una excepción en SendMessageFunction: {ex.Message}");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
        
        
    }
}
