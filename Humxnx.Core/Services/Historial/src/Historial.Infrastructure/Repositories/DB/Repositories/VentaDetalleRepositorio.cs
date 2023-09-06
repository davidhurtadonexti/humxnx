using System;
using Humxnx.Historial.Core.Domain.Entities;
using Humxnx.Historial.Core.Domain.Repositories;
using Humxnx.Historial.Core.Infrastructure.Repositories.DB.Contexts;

namespace Humxnx.Historial.Core.Infrastructure.Repositories.DB.Repositories;

public class VentaDetalleRepositorio : IRepositorioDetalle<VentaDetalle, Guid>
{
    private readonly VentaContexto db;

    public VentaDetalleRepositorio(VentaContexto _db)
    {
        db = _db;
    }

    public VentaDetalle Agregar(VentaDetalle entidad)
    {
        db.VentaDetalles.Add(entidad);
        return entidad;
    }

    public void GuardarTodosLosCambios()
    {
        db.SaveChanges();
    }
}