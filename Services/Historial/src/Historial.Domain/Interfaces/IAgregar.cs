namespace Humxnx.Historial.Core.Domain.Interfaces;

public interface IAgregar<TEntidad>
{
    TEntidad Agregar(TEntidad entidad);
}