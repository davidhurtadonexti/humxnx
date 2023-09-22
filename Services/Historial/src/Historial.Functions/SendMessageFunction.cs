using Azure.Messaging.ServiceBus;
using Humxnx.Historial.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Humxnx.Historial.Core.Functions;

public static class SendMessageFunction
{


    [FunctionName("SendMessageFunction")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "send-message")] HttpRequest req,
        ILogger logger)
    {
        
        // Leer y validar el cuerpo de la solicitud JSON.
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        // var inputModel = JsonConvert.DeserializeObject<QueueMessage>(requestBody);
        // ValidationWrapper<QueueMessage> httpResponseBody = await req.GetBodyAsync<QueueMessage>();
        //
        // if (!httpResponseBody.IsValid)
        // {
        //     return new BadRequestObjectResult($"Invalid input: {string.Join(", ", httpResponseBody.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
        // }
        //


        try
        {
            var connectionString = Environment.GetEnvironmentVariable("AzureServiceBusConnectionString");
            const string queueName = "success_order_queue"; 
            await using var client = new ServiceBusClient(connectionString);

            var sender = client.CreateSender(queueName);
            var message = new ServiceBusMessage(requestBody);
            await sender.SendMessageAsync(message);
            // await sender.SendMessageAsync(message);
            await sender.CloseAsync();
            await client.DisposeAsync();
            logger.LogInformation( "Envia mensaje al Service Bus");
            return new OkObjectResult($"Mensaje enviado");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Ocurrió una excepción en SendMessageFunction: {ex.Message}");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
    
    private static ValidationResult ValidateInputModel(QueueMessage inputModel)
    {
        // Realiza la validación del modelo aquí.
        // Puedes usar DataAnnotations u otras técnicas de validación.

        var context = new ValidationContext(inputModel, null, null);
        var results = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(inputModel, context, results, true);

        if (!isValid)
        {
            var errorMessages = results.Select(result => result.ErrorMessage);
            return new ValidationResult(string.Join(", ", errorMessages));
        }

        return ValidationResult.Success;
    }
    
}