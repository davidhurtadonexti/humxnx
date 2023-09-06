using System;
using System.Collections.Generic;
using System.Linq;
using Humxnx.Historial.Core.Domain.Entities;
using Humxnx.Historial.Core.Domain.Repositories;
using Humxnx.Historial.Core.Infrastructure.Repositories.DB.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Humxnx.Historial.Core.Infrastructure.Repositories.DB.Repositories;

public class VentaRepositorio : IRepositorioMovimiento<Venta, Guid>
{
    private readonly VentaContexto db;

    public VentaRepositorio(VentaContexto _db)
    {
        db = _db;
    }

    public Venta Agregar(Venta entidad)
    {
        entidad.ventaId = Guid.NewGuid();
        db.Ventas.Add(entidad);
        return entidad;
    }

    public void Anular(Guid entidadId)
    {
        var ventaSeleccionada = db.Ventas.Where(c => c.ventaId == entidadId).FirstOrDefault();
        if (ventaSeleccionada == null)
            throw new NullReferenceException("Esta intentando anular una venta que no existe 😡😡😡");

        ventaSeleccionada.anulado = true;
        db.Entry(ventaSeleccionada).State = EntityState.Modified;
    }

    public void GuardarTodosLosCambios()
    {
        db.SaveChanges();
    }

    public List<Venta> Listar()
    {
        return db.Ventas.ToList();
    }

    public Venta SeleccionarPorID(Guid entidadId)
    {
        var ventaSeleccionada = db.Ventas.Where(c => c.ventaId == entidadId).FirstOrDefault();
        return ventaSeleccionada;
    }
}