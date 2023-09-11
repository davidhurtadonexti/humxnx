using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
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
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

public static class ReactiveApiFunction
{
    // Utilizaremos un BehaviorSubject para mantener un flujo de eventos simulados
    private static readonly Dictionary<string, Subject<string>> SessionEventStreams = new Dictionary<string, Subject<string>>();
    private static readonly List<TextWriter> clients = new();

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
        // response.Headers.Add("Cache-Control", "no-cache");
        response.Headers.Add("Connection", "keep-alive");
        string sessionStateId = sessionId;
        var client = new StreamWriter(response.Body);
        clients.Add(client);
        // Obtener el estado de la sesión desde Azure Blob Storage
        //var sessionStateId = await GetSessionStateFromStorage(sessionId, log);
        // Verifica si ya existe un flujo de eventos para esta sesión
        var checkSession = SessionEventStreams.TryGetValue(sessionStateId, out var chekckEventObserver);
        if (!checkSession)
        {
          
            // Si no existe, crea uno nuevo para esta sesión
            var newEventStream = new Subject<string>();
            chekckEventObserver = newEventStream;

            // Agrega el nuevo flujo de eventos al diccionario
            SessionEventStreams.Add(sessionStateId, chekckEventObserver);
            
            // Genera un ID de sesión único (puedes usar cualquier lógica de generación que necesites)
            // string newSessionId = Guid.NewGuid().ToString();

            // El estado de sesión inicial

            // Guarda el estado de sesión en Azure Blob Storage
            //await SaveSessionStateToStorage(sessionStateId, sessionStateId, log);

            log.LogInformation("Init session: " + sessionStateId);
            var messageData = new { data = "Stream creado exitosamente con SessionID: "+ sessionStateId};
            await client.WriteAsync($"data: {messageData}\n\n");
            await client.FlushAsync();
            
            log.LogInformation("Successfully SessionId: " + sessionStateId);
        }
        else
        {  
            log.LogInformation("Sesion existente con SessionID " + sessionStateId);
            var messageData = new { data = "Sesion existente con SessionID: "+ sessionStateId};
            await client.WriteAsync($"data: {messageData}\n\n");
            await client.FlushAsync();
        }
        // Elimina el flujo de eventos de la sesión cuando se cierra la conexión
        response.OnCompleted(() =>
        {
            log.LogInformation("dentro de response.OnCompleted");
            clients.Remove(client);
            client.Dispose();
            SessionEventStreams.Remove(sessionStateId);
            return Task.CompletedTask;
        });

        try
        {
            log.LogInformation($"data: {sessionStateId}", checkSession);
            // if (checkSession)
            var checkSessionCreated = SessionEventStreams.TryGetValue(sessionStateId, out var eventObserver);
            if (checkSessionCreated)
            {
                log.LogInformation("dentro de SessionEventStreams");
                // Suscríbete al flujo de eventos y envía eventos al cliente
                await eventObserver.ForEachAsync(async (eventData) =>
                {
                    try
                    {
                        log.LogInformation("fuera de eventData arriba");
                        if (eventData != null)
                        {
                            log.LogInformation("Se levanta el Observador para la SessionID: " + sessionStateId);
                            var messageData = new { data = eventData };
                            var eventDataJsons = JsonSerializer.Serialize(messageData);
                            await client.WriteAsync($"data: {eventDataJsons}\n\n");
                            await client.FlushAsync();
                            log.LogInformation("Escribiendo mensaje para la SessionId: " + sessionStateId);
                        }
                        log.LogInformation("fuera de eventData abajo");
                    }
                    catch (Exception ex)
                    {
                        log.LogError($"Error en la transmisión de eventos para la sesión {sessionStateId}: {ex.Message}");
                        // Puedes manejar la excepción de acuerdo a tus necesidades, como cerrar la conexión o tomar otra acción.
                    }
                });
                
            }
            
        }
        catch (Exception ex)
        {
            log.LogError($"Error en la transmisión de eventos: {ex.Message}");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
        finally
        {
            
            // chekckEventObserver.OnCompleted();
            clients.Remove(client);
            await client.DisposeAsync();
 
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
    
    private static async Task SaveSessionStateToStorage(string sessionId, string sessionState, ILogger log)
    {
        string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        string containerName = "session-container-test"; // Nombre del contenedor en Azure Blob Storage

        try
        {
            // Crear una instancia del cliente de Blob Storage
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Obtener una referencia al contenedor
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Obtener una referencia al blob que contendrá el estado de la sesión
            BlobClient blobClient = containerClient.GetBlobClient($"{sessionId}.json");

            // Convertir el estado de sesión en bytes
            byte[] sessionStateBytes = Encoding.UTF8.GetBytes(sessionState);

            // Cargar los bytes en el blob
            using (MemoryStream stream = new MemoryStream(sessionStateBytes))
            {
                await blobClient.UploadAsync(stream, true);
            }
        }
        catch (RequestFailedException ex)
        {
            log.LogError($"Error al acceder a Azure Blob Storage: {ex.Message}");
            throw; // Manejar adecuadamente el error según tus necesidades
        }
    }
    
    private static async Task<string> GetSessionStateFromStorage(string sessionId, ILogger log)
    {
        string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        string containerName = "session-container-test"; 
        try
        {
            // Crear una instancia del cliente de Blob Storage
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Obtener una referencia al contenedor
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Obtener una referencia al blob que contiene el estado de la sesión
            BlobClient blobClient = containerClient.GetBlobClient($"{sessionId}.json");

            // Verificar si el blob existe
            if (await blobClient.ExistsAsync())
            {
                // Descargar el contenido del blob
                BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();

                // Leer el contenido y devolverlo como string
                using (StreamReader reader = new StreamReader(blobDownloadInfo.Content))
                {
                    return await reader.ReadToEndAsync();
                }
            }

            // Si el blob no existe, puedes devolver un estado de sesión inicial
            return  sessionId;
        }
        catch (RequestFailedException ex)
        {
            log.LogError($"Error al acceder a Azure Blob Storage: {ex.Message}");
            throw; // Manejar adecuadamente el error según tus necesidades
        }
    }
}
