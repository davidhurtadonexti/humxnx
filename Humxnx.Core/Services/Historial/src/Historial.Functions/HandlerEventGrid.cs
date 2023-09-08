
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Azure.Messaging.EventGrid;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Humxnx.Historial.Core.Functions;

public static class HandlerEventGrid
{
    
    [FunctionName("Consumer")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "consumer")] HttpRequest req,
        ILogger log)
    {
        try
        {
            
            string connectionString = Environment.GetEnvironmentVariable("AzureServiceBusConnectionString");
            string queueName = "success_order_queue"; 
            string requestQueueBody = await new StreamReader(req.Body).ReadToEndAsync();
            await using var client = new ServiceBusClient(connectionString);

            var receiver = client.CreateReceiver(queueName);

            //var message = new ServiceBusMessage(requestQueueBody);
            receiver.ReceiveMessageAsync();

            //tring requestBody = result.Body.ToString();
            // Leer el cuerpo JSON del evento de Event Grid
            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // Procesa el evento de Event Grid y conviértelo en un evento SSE
            string sseEvent = $"data: {req.Body}\n\n";

            // Configura la respuesta SSE
            return new ContentResult
            {
                Content = sseEvent,
                ContentType = "text/event-stream",
                StatusCode = (int)HttpStatusCode.OK
            };
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error procesando el evento de Event Grid");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}