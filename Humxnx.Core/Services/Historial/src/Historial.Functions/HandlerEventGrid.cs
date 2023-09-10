using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Azure.Messaging.EventGrid;
using Humxnx.Historial.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

public static class ReactiveApiFunction
{
    // Utilizaremos un BehaviorSubject para mantener un flujo de eventos simulados
    private static readonly Dictionary<string, Subject<string>> sessionEventStreams = new Dictionary<string, Subject<string>>();


    [FunctionName("ObservableEventGrid")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "observable/{sessionId}")] HttpRequest req,
        string sessionId,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");
        // Establece la respuesta HTTP como un flujo de eventos (Content-Type: text/event-stream)
        var response = req.HttpContext.Response;
        response.Headers.Add("Content-Type", "text/event-stream");
        
        // Verifica si ya existe un flujo de eventos para esta sesión
        var checkSession = sessionEventStreams.TryGetValue(sessionId, out var ChekckEventObserver);
        if (!checkSession)
        {
          
            // Si no existe, crea uno nuevo para esta sesión
            var newEventStream = new Subject<string>();
            ChekckEventObserver = newEventStream;

            // Agrega el nuevo flujo de eventos al diccionario
            sessionEventStreams.Add(sessionId, ChekckEventObserver);
            log.LogInformation("Init session: " + sessionId);
            var messageData = new { data = "Stream creado exitosamente con SessionID: "+ sessionId};
            await response.WriteAsync($"data: {messageData}\n\n");
            await response.Body.FlushAsync();
            log.LogInformation("Async response sessionId: " + sessionId);
            // Elimina el flujo de eventos de la sesión cuando se cierra la conexión
            response.OnCompleted(() =>
            {
                sessionEventStreams.Remove(sessionId);
                return Task.CompletedTask;
            });
            log.LogInformation("Correctt flow SessionId: " + sessionId);
            // return new OkResult();
        }
        
    

        try
        {
            if (sessionEventStreams.TryGetValue(sessionId, out var eventObserver))
            {
                // Suscríbete al flujo de eventos y envía eventos al cliente
                async void OnNext(string eventData)
                {
                    // Envía eventos al cliente en un formato adecuado para EventSource

                    if (eventData != null)
                    {
                        log.LogInformation("Se levanta el Observador para la SessionID: " + sessionId);
                        var messageData = new { data = eventData };
                        var eventDataJsons = JsonSerializer.Serialize(messageData);
                        await response.WriteAsync($"data: {eventDataJsons}\n\n");
                        await response.Body.FlushAsync();
                    }
                }

                await eventObserver.ForEachAsync(OnNext);
            }

            return new OkResult();
        }
        catch (Exception ex)
        {
            log.LogError($"Error en la transmisión de eventos: {ex.Message}");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
    
    [FunctionName("ReceiveEventGridMessage")]
    public static void Receive(
        [EventGrid(TopicEndpointUri = "EventGridAttribute.TopicEndpointUri", TopicKeySetting = "EventGridAttribute.TopicKeySetting")] IAsyncCollector<EventGridEvent> eventCollector,
        [ServiceBusTrigger("success_order_queue", Connection = "AzureServiceBusConnectionString")] string message,
        ILogger logger)
    {
        try
        {
            QueueMessage messageReceived = JsonSerializer.Deserialize<QueueMessage>(message);
            logger.LogInformation($"Se captura el mensaje del EventGrid por medio del Service bus: {message}");
         
            if (sessionEventStreams.TryGetValue(messageReceived.SessionId, out var eventObserver))
            {
                logger.LogInformation("response SessionId: " + messageReceived.SessionId);
                 eventObserver.OnNext(message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al recibir mensajes: {ex.Message}");
        }
    }
}
