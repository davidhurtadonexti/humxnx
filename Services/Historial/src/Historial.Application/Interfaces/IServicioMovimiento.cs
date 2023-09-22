using Humxnx.Historial.Core.Domain.Interfaces;

namespace Humxnx.Historial.Core.Application.Interfaces;

public interface IServicioMovimiento<TEntidad, TEntidadID>
    : IAgregar<TEntidad>, IListar<TEntidad, TEntidadID>
{
    void Anular(TEntidadID entidadId);
}