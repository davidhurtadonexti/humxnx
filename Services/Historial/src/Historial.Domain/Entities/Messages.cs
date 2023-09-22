using System.ComponentModel.DataAnnotations;

namespace Humxnx.Historial.Core.Domain.Entities;

public class QueueMessage
{
    [Required(ErrorMessage = "El campo 'message' es obligatorio.")]
    public string Message { get; set; }
    public string SessionId { get; set; }
}