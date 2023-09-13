
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.EventGrid;
using Humxnx.Historial.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;


namespace Humxnx.Historial.Core.Functions;


public static class HandlerEventGrid
{
    // private static readonly Subject<string> messageSubject = new Subject<string>();
    private static readonly Dictionary<string, Subject<string>> SessionEventStreams = new Dictionary<string, Subject<string>>();
    
    [FunctionName("Observable")]
    public static async Task<IActionResult>  Consumer(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "observable/{sessionId}")] HttpRequest req,
        string sessionId,
        ILogger log)
    {
        
        var response = req.HttpContext.Response;
        response.Headers.Add("Content-Type", "text/event-stream");
        response.Headers.Add("Cache-Control", "no-cache");
        response.Headers.Add("Connection", "keep-alive");
        var sessionStateId = sessionId;
        
        var checkSession = SessionEventStreams.TryGetValue(sessionStateId, out var chekckEventObserver);
        if (!checkSession)
        {
            // Si no existe, crea uno nuevo para esta sesión
            var newEventStream = new Subject<string>();
            chekckEventObserver = newEventStream;
            // Agrega el nuevo flujo de eventos al diccionario
            SessionEventStreams.Add(sessionStateId, chekckEventObserver);

            log.LogInformation("Init session: " + sessionStateId);
            var messageData = new { data = "Stream creado exitosamente con SessionID: "+ sessionStateId};
            var eventDataJsons = JsonSerializer.Serialize(messageData);

            await response.WriteAsync($"data: {eventDataJsons}\n\n", Encoding.UTF8);
            await response.Body.FlushAsync();
            await response.Body.FlushAsync();
            await response.Body.FlushAsync();
            await response.WriteAsync("\r\n", Encoding.UTF8);
            await response.Body.FlushAsync();
            await response.Body.FlushAsync();
            await response.Body.FlushAsync();
            await response.Body.FlushAsync();
            log.LogInformation("Successfully SessionId: " + sessionStateId);
        }
        else
        {
            log.LogInformation("Sesion existente con SessionID " + sessionStateId);
            var messageData = new { data = "Sesion existente con SessionID: " + sessionStateId };
            var eventDataJsons = JsonSerializer.Serialize(messageData);

            await response.WriteAsync($"data: {eventDataJsons}\n\n", Encoding.UTF8);
            await response.Body.FlushAsync();
            await response.Body.FlushAsync();
            await response.Body.FlushAsync();
            await response.WriteAsync("\r\n", Encoding.UTF8);
            await response.Body.FlushAsync();
            await response.Body.FlushAsync();
            await response.Body.FlushAsync();
            await response.Body.FlushAsync();
        }
        
        // var messageData = new { data = "Stream creado exitosamente"};
        // var eventDataJsons = JsonSerializer.Serialize(messageData);
        // // Escribe los datos JSON seguidos de "\r\n"
        //
        // await response.WriteAsync($"data: {eventDataJsons}\n\n", Encoding.UTF8);
        // await response.Body.FlushAsync();
        // await response.Body.FlushAsync();
        // await response.WriteAsync("\r\n", Encoding.UTF8);
        // await response.Body.FlushAsync();
        // await response.Body.FlushAsync();
        // await response.WriteAsync("\r\n");
        // await response.Body.FlushAsync();
        // await response.Body.FlushAsync();
        try{
            var checkSessionCreated = SessionEventStreams.TryGetValue(sessionStateId, out var eventObserver);
            if (checkSessionCreated)
            {
                // Crea un Observable a partir del Subject
                var sseObservable = eventObserver.AsObservable();
                // Suscríbete al Observable y envía eventos SSE al cliente
                sseObservable.Subscribe(async sseEvent =>
                {
                    var messageEventData = new { data = sseEvent };
                    var dataJsons = JsonSerializer.Serialize(messageEventData);
                    // Envía el evento SSE al cliente de forma asincrónica
                    await response.WriteAsync($"data: {dataJsons}\n\n", Encoding.UTF8);
                    await response.Body.FlushAsync();
                    await response.Body.FlushAsync();
                    await response.Body.FlushAsync();
                    await response.WriteAsync("\r\n", Encoding.UTF8);
                    await response.Body.FlushAsync();
                    await response.Body.FlushAsync();
                    await response.Body.FlushAsync();
                    await response.Body.FlushAsync();
                });
                
                // Espera a que la suscripción al Observable termine antes de responder
                await sseObservable.ToTask();
            }
        
        }
        catch (Exception ex)
        {
            log.LogError($"Error en la transmisión de eventos: {ex.Message}");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        return new OkResult();
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
         
            if (SessionEventStreams.TryGetValue(messageReceived.SessionId, out var eventObserver))
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
