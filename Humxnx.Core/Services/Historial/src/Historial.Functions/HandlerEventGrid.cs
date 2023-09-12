using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public static class ReactiveFunctionWithSSE
{
    private static readonly ConcurrentQueue<string> eventQueue = new ConcurrentQueue<string>();
    private static readonly List<StreamWriter> clients = new List<StreamWriter>();

    [FunctionName("sendEvent")]
    public static async Task<IActionResult> SendEvent(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "sendEvent")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("Received an event.");

        // Simular un evento y agregarlo a la cola de eventos
        var evento = new { message = "Este es un evento" };
        var eventJson = JsonConvert.SerializeObject(evento);
        eventQueue.Enqueue(eventJson);

        // Notificar a los clientes
        NotifyClients(eventJson);

        return new OkResult();
    }

    [FunctionName("streamEvents")]
    public static async Task<IActionResult> StreamEvents(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "streamEvents")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("Client connected for streaming events.");

        // Establecer la respuesta HTTP como Server-Sent Events (SSE)
        var response = req.HttpContext.Response;
        response.Headers.Add("Content-Type", "text/event-stream");
        response.Headers.Add("Cache-Control", "no-cache");
        response.Headers.Add("Connection", "keep-alive");

        // Crear un StreamWriter para escribir eventos en la respuesta
        var clientStreamWriter = new StreamWriter(response.Body);
        clients.Add(clientStreamWriter);

        // Mantener la conexión abierta
        while (!response.HttpContext.RequestAborted.IsCancellationRequested)
        {
            await response.Body.FlushAsync();
            await Task.Delay(1000); // Intervalo de envío de eventos
        }

        // Remover el StreamWriter cuando la conexión se cierra
        clients.Remove(clientStreamWriter);

        log.LogInformation("Client disconnected.");

        return new StatusCodeResult((int)HttpStatusCode.OK);
    }

    private static void NotifyClients(string eventJson)
    {
        // Enviar el evento a todos los clientes conectados
        foreach (var clientStreamWriter in clients)
        {
            clientStreamWriter.WriteLine($"data: {eventJson}\n");
            clientStreamWriter.Flush();
        }
    }
}
