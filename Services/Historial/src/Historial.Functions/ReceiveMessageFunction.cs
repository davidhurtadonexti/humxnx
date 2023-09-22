using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace Humxnx.Historial.Core.Functions;
public static class ReceiveMessageFunction
{
    //[FunctionName("ReceiveMessage")]
    public static void Run(
        [ServiceBusTrigger("success_order_queue", Connection = "AzureServiceBusConnectionString")] string message,
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