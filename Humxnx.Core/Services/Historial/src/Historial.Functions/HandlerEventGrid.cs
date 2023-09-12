
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventGrid;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Humxnx.Historial.Core.Functions;

public static class HandlerEventGrid
{
    private static readonly Subject<string> messageSubject = new Subject<string>();
    private static readonly List<StreamWriter> clients = new List<StreamWriter>();
    
    [FunctionName("Consumer")]
    public static async Task<IActionResult> Consumer(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "observable")] HttpRequest req,
        ILogger log)
    {

        var response = req.HttpContext.Response;
        response.Headers.Add("Content-Type", "text/event-stream");
        response.Headers.Add("Cache-Control", "no-cache");
        response.Headers.Add("Connection", "keep-alive");

        var client = new StreamWriter(response.Body);
        clients.Add(client);
        try
        {
            var messageData = "Stream creado exitosamente";
            var message = $"data: {messageData}\n\n";
            await client.WriteAsync(message);
            await client.FlushAsync();
            await Task.Delay(2000);
            while (!req.HttpContext.RequestAborted.IsCancellationRequested)
            {
                
                // Obtiene el último mensaje del flujo reactivo
                string latestMessage = messageSubject.FirstOrDefault();

                if (latestMessage != null)
                {
                    var eventDataJson = JsonConvert.SerializeObject(latestMessage);
                    await client.WriteAsync($"data: {eventDataJson}\n\n");
                    await client.FlushAsync();
                    // await client.FlushAsync();
                }
           
                await Task.Delay(5000); // Espera 2 segundos antes de enviar el siguiente mensaje
            }
        }
        catch (Exception)
        {
            // Maneja las desconexiones del cliente u otros errores aquí
        }
        finally
        {
            clients.Remove(client);
            client.Dispose();
        }

        return new EmptyResult();
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