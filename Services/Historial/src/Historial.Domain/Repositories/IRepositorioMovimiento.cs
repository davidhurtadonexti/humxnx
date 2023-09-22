using Humxnx.Historial.Core.Domain.Interfaces;

namespace Humxnx.Historial.Core.Domain.Repositories;

public interface IRepositorioMovimiento<TEntidad, TEntidadID>
    : IAgregar<TEntidad>, IListar<TEntidad, TEntidadID>, ITransaccion
{
    void Anular(TEntidadID entidadId);
}