namespace Humxnx.Historial.Core.Domain.Interfaces;

public interface IEliminar<TEntidadID>
{
    void Eliminar(TEntidadID entidadId);
}