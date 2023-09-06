using Humxnx.Historial.Core.Domain.Interfaces;

namespace Humxnx.Historial.Core.Application.Interfaces;

public interface IServicioBase<TEntidad, TEntidadID>
    : IAgregar<TEntidad>, IEditar<TEntidad>, IEliminar<TEntidadID>, IListar<TEntidad, TEntidadID>
{
}