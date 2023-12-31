﻿using System;
using System.Collections.Generic;
using Humxnx.Historial.Core.Application.Interfaces;
using Humxnx.Historial.Core.Domain.Entities;
using Humxnx.Historial.Core.Domain.Repositories;

namespace Humxnx.Historial.Core.Application.Services;

public class ProductoServicio : IServicioBase<Producto, Guid>
{
    private readonly IRepositorioBase<Producto, Guid> repoProducto;

    public ProductoServicio(IRepositorioBase<Producto, Guid> _repoProducto)
    {
        repoProducto = _repoProducto;
    }

    public Producto Agregar(Producto entidad)
    {
        if (entidad == null)
            throw new ArgumentNullException("El 'Producto' es requerido");

        var resultProducto = repoProducto.Agregar(entidad);
        repoProducto.GuardarTodosLosCambios();
        return resultProducto;
    }

    public void Editar(Producto entidad)
    {
        if (entidad == null)
            throw new ArgumentNullException("El 'Producto' es requerido para editar");

        repoProducto.Editar(entidad);
        repoProducto.GuardarTodosLosCambios();
    }

    public void Eliminar(Guid entidadId)
    {
        repoProducto.Eliminar(entidadId);
        repoProducto.GuardarTodosLosCambios();
    }

    public List<Producto> Listar()
    {
        return repoProducto.Listar();
    }

    public Producto SeleccionarPorID(Guid entidadId)
    {
        return repoProducto.SeleccionarPorID(entidadId);
    }
}