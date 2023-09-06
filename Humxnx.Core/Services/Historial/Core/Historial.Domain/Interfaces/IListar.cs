using System.Collections.Generic;

namespace Humxnx.Historial.Core.Domain.Interfaces;

public interface IListar<TEntidad, TEntidadID>
{
    List<TEntidad> Listar();

    TEntidad SeleccionarPorID(TEntidadID entidadId);
}