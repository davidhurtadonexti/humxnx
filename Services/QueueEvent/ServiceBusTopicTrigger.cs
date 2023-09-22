using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.EventGrid;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue.Protocol;

namespace QueueEvent;

public static class ServiceBusTopicTrigger
{
    
    [FunctionName("ReceiveEventGridMessage")]
    public static async Task Receive(
        [EventGrid(TopicEndpointUri = "EventGridAttribute.TopicEndpointUri", TopicKeySetting = "EventGridAttribute.TopicKeySetting")] IAsyncCollector<EventGridEvent> eventCollector,
        [ServiceBusTrigger("success_order_queue", Connection = "AzureServiceBusConnectionString")] string message,
        ILogger logger)
    {
            logger.LogInformation($"Se captura el mensaje del EventGrid por medio del Service bus: {message}");
            try
            {
                var connectionString = Environment.GetEnvironmentVariable("AzureServiceBusConnectionString");
                const string queueName = "reactive_process"; 
                await using var client = new ServiceBusClient(connectionString);
                var sender = client.CreateSender(queueName);
                var messageBus = new ServiceBusMessage(message);
                await sender.SendMessageAsync(messageBus);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Ocurrió una excepción en SendMessageFunction: {ex.Message}");
            }
    }
}