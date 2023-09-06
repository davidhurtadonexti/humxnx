using Humxnx.Historial.Core.Domain.Interfaces;

namespace Humxnx.Historial.Core.Domain.Repositories;

public interface IRepositorioBase<TEntidad, TEntidadID>
    : IAgregar<TEntidad>, IEditar<TEntidad>, IEliminar<TEntidadID>, IListar<TEntidad, TEntidadID>, ITransaccion
{
}