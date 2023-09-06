using Humxnx.Historial.Core.Domain.Interfaces;

namespace Humxnx.Historial.Core.Domain.Repositories;

public interface IRepositorioDetalle<TEntidad, TMovimientoID>
    : IAgregar<TEntidad>, ITransaccion
{
}