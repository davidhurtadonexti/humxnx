using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Humxnx.Historial.Core.Functions;
public static class ReceiveMessageFunction
{
    [FunctionName("ReceiveMessage")]
    public static void Run(
        [ServiceBusTrigger("success_order_queue", Connection = "AzureServiceBusConnectionString")] string message,
        // [HttpTrigger(AuthorizationLevel.Function, "get", Route = "receive-message")] HttpRequest req,
        ILogger logger)
    {

        try
        {
            logger.LogInformation($"Mensaje recibido: {message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al recibir mensajes: {ex.Message}");
        }
    }
}