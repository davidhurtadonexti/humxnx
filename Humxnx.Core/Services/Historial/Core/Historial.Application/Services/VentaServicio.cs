﻿using System;
using System.Collections.Generic;
using Humxnx.Historial.Core.Application.Interfaces;
using Humxnx.Historial.Core.Domain.Entities;
using Humxnx.Historial.Core.Domain.Repositories;

namespace Humxnx.Historial.Core.Application.Services;

public class VentaServicio : IServicioMovimiento<Venta, Guid>
{
    private readonly IRepositorioDetalle<VentaDetalle, Guid> repoDetalle;
    private readonly IRepositorioBase<Producto, Guid> repoProducto;

    private readonly IRepositorioMovimiento<Venta, Guid> repoVenta;

    public VentaServicio(
        IRepositorioMovimiento<Venta, Guid> _repoVenta,
        IRepositorioBase<Producto, Guid> _repoProducto,
        IRepositorioDetalle<VentaDetalle, Guid> _repoDetalle
    )
    {
        repoVenta = _repoVenta;
        repoProducto = _repoProducto;
        repoDetalle = _repoDetalle;
    }

    public Venta Agregar(Venta entidad)
    {
        if (entidad == null)
            throw new ArgumentNullException("La 'Venta' es requerida");

        var ventaAgregada = repoVenta.Agregar(entidad);

        entidad.VentaDetalles.ForEach(detalle =>
        {
            var productoSeleccionado = repoProducto.SeleccionarPorID(detalle.productoId);
            if (productoSeleccionado == null)
                throw new NullReferenceException("Usted esta intentando vender un producto que no existe 😡😡😡");


            detalle.costoUnitario = productoSeleccionado.costo;
            detalle.precioUnitario = productoSeleccionado.precio;
            detalle.subtotal = detalle.precioUnitario * detalle.cantidadVendida;
            detalle.impuesto = detalle.subtotal * 15 / 100;
            detalle.total = detalle.subtotal + detalle.impuesto;
            repoDetalle.Agregar(detalle);

            productoSeleccionado.cantidadEnStock -= detalle.cantidadVendida;
            repoProducto.Editar(productoSeleccionado);

            entidad.subtotal += detalle.subtotal;
            entidad.impuesto += detalle.impuesto;
            entidad.total += detalle.total;
        });

        repoVenta.GuardarTodosLosCambios();
        return entidad;
    }

    public void Anular(Guid entidadId)
    {
        repoVenta.Anular(entidadId);
        repoVenta.GuardarTodosLosCambios();
    }

    public List<Venta> Listar()
    {
        return repoVenta.Listar();
    }

    public Venta SeleccionarPorID(Guid entidadId)
    {
        return repoVenta.SeleccionarPorID(entidadId);
    }
}