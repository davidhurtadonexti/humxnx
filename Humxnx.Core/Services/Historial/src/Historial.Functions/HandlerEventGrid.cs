
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Azure.Messaging.EventGrid;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Humxnx.Historial.Core.Functions;

public static class HandlerEventGrid
{
    private static readonly Subject<string> messageSubject = new Subject<string>();
    private static readonly List<TextWriter> clients = new();

    [FunctionName("Consumer")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "observable")]
        HttpRequest req,
        ILogger log)
    {
        log.LogInformation("Esperando mensajes desde un evento en eventGrid para responder a la solicitud HTTP desde un Observador.");

        var response = req.HttpContext.Response;
        response.Headers.Add("Content-Type", "text/event-stream");
        response.Headers.Add("Cache-Control", "no-cache");
        response.Headers.Add("Connection", "keep-alive");

        var client = new StreamWriter(response.Body);
        clients.Add(client);

        try
        {
            // Conexión para enviar eventos SSE a los clientes cuando lleguen eventos de Event Grid.
            while (!req.HttpContext.RequestAborted.IsCancellationRequested)
            {
                // Obtiene el  mensaje del flujo reactivo
                string latestMessage = messageSubject.FirstOrDefault();

                if (latestMessage != null)
                {
                    log.LogInformation("Enviando al Observador el mensaje leído");
                    var eventData = new { data = latestMessage };
                    var eventDataJsons = JsonConvert.SerializeObject(eventData);
                    await client.WriteAsync($"data: {eventDataJsons}\n\n");
                }
                
                await client.FlushAsync();
            }
        }
        catch (Exception ex)
        {
            log.LogError($"Error en la transmisión de SSE: {ex.Message}");
        }
        finally
        {
            clients.Remove(client);
            client.Dispose();
        }

        return new OkResult();
    }
    
    
    [FunctionName("ReceiveMessage")]
    public static void Receive(
        [EventGrid(TopicEndpointUri = "EventGridAttribute.TopicEndpointUri", TopicKeySetting = "EventGridAttribute.TopicKeySetting")] IAsyncCollector<EventGridEvent> eventCollector,
        [ServiceBusTrigger("success_order_queue", Connection = "AzureServiceBusConnectionString")] string message,
        ILogger logger)
    {
        try
        {

            logger.LogInformation($"Mensaje recibido: {message}");
            messageSubject.OnNext(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al recibir mensajes: {ex.Message}");
        }
    }
}