using System;
using System.Collections.Generic;
using System.Linq;
using Humxnx.Historial.Core.Domain.Entities;
using Humxnx.Historial.Core.Domain.Repositories;
using Humxnx.Historial.Core.Infrastructure.Repositories.DB.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Humxnx.Historial.Core.Infrastructure.Repositories.DB.Repositories;

public class ProductoRepositorio : IRepositorioBase<Producto, Guid>
{
    private readonly VentaContexto db;

    public ProductoRepositorio(VentaContexto _db)
    {
        db = _db;
    }

    public Producto Agregar(Producto entidad)
    {
        entidad.productoId = Guid.NewGuid();
        db.Productos.Add(entidad);
        return entidad;
    }

    public void Editar(Producto entidad)
    {
        var productoSeleccionado = db.Productos.Where(c => c.productoId == entidad.productoId).FirstOrDefault();
        if (productoSeleccionado != null)
        {
            productoSeleccionado.nombre = entidad.nombre;
            productoSeleccionado.descripcion = entidad.descripcion;
            productoSeleccionado.costo = entidad.costo;
            productoSeleccionado.precio = entidad.precio;
            productoSeleccionado.cantidadEnStock = entidad.cantidadEnStock;

            db.Entry(productoSeleccionado).State = EntityState.Modified;
        }
    }

    public void Eliminar(Guid entidadId)
    {
        var productoSeleccionado = db.Productos.Where(c => c.productoId == entidadId).FirstOrDefault();
        if (productoSeleccionado != null) db.Productos.Remove(productoSeleccionado);
    }

    public void GuardarTodosLosCambios()
    {
        db.SaveChanges();
    }

    public List<Producto> Listar()
    {
        var products = new List<Producto>();
        products.Add(new Producto{
            productoId = new Guid("d4e3be27-dc7f-4f52-b4bb-41c1ea55eb1a"),
            nombre = "Plan 1",
            descripcion = "Description 1",
            costo = (decimal)150.60
        });

        products.Add(new Producto{
            productoId = new Guid("3ed9e3c3-8383-417d-81e9-e9e574346662"),
            nombre = "Plan 2",
            descripcion = "Description 2",
            costo = (decimal)102.40
        });
        // return db.Productos.ToList();
        return products;
    }

    public Producto SeleccionarPorID(Guid entidadId)
    {
        var productoSeleccionado = db.Productos.Where(c => c.productoId == entidadId).FirstOrDefault();
        return productoSeleccionado;
    }
}