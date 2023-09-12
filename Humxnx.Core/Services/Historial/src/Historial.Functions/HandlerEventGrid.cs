
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;


namespace Humxnx.Historial.Core.Functions;


public static class HandlerEventGrid
{
    private static readonly Subject<string> messageSubject = new Subject<string>();
    
    [FunctionName("Consumer")]
    public static async Task<IActionResult>  Consumer(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "observable")] HttpRequest req,
        ILogger log)
    {
        
        var response = req.HttpContext.Response;
        response.Headers.Add("Content-Type", "text/event-stream");
        response.Headers.Add("Cache-Control", "no-cache");
        response.Headers.Add("Connection", "keep-alive");

        // Crea un Observable a partir del Subject
        var sseObservable = messageSubject.AsObservable();
        
        var messageData = new { data = "Stream creado exitosamente"};
        var eventDataJsons = JsonSerializer.Serialize(messageData);
        // Escribe los datos JSON seguidos de "\r\n"
        await response.WriteAsync($"{eventDataJsons}\r\n");
        await response.WriteAsync($"{eventDataJsons}\n\n");
        
        await response.Body.FlushAsync();

        // Suscríbete al Observable y envía eventos SSE al cliente
        sseObservable.Subscribe(async sseEvent =>
        {
            var messageData = new { data = sseEvent };
            var eventDataJsons = JsonSerializer.Serialize(messageData);
            // Envía el evento SSE al cliente de forma asincrónica
            await response.WriteAsync($"{eventDataJsons}\r\n");
            await response.WriteAsync($"data: {eventDataJsons}\n\n", Encoding.UTF8);
            await response.Body.FlushAsync();
        });

        // Espera a que la suscripción al Observable termine antes de responder
        await sseObservable.ToTask();

        return new OkResult();
    }
    
    [FunctionName("ReceiveMessage")]
    public static void Receive(
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
