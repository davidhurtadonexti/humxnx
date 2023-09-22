using System;
using System.Collections.Generic;
using Humxnx.Historial.Core.Application.Services;
using Humxnx.Historial.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

// using Historial.Application.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Humxnx.Historial.Core.Infrastructure.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VentaController : ControllerBase
{
    private VentaServicio CrearServicio()
    {
        var servicio = new VentaServicio(null, null, null);
        return servicio;
    }

    // GET: api/<VentaController>
    [HttpGet]
    public ActionResult<List<Venta>> Get()
    {
        var servicio = CrearServicio();
        return Ok(servicio.Listar());
    }

    // GET api/<VentaController>/5
    [HttpGet("{id}")]
    public ActionResult<Venta> Get(Guid id)
    {
        var servicio = CrearServicio();
        return Ok(servicio.SeleccionarPorID(id));
    }

    // POST api/<VentaController>
    [HttpPost]
    public ActionResult Post([FromBody] Venta venta)
    {
        var servicio = CrearServicio();
        servicio.Agregar(venta);
        return Ok("Venta guardada correctamente!!!!");
    }

    // DELETE api/<VentaController>/5
    [HttpDelete("{id}")]
    public ActionResult Delete(Guid id)
    {
        var servicio = CrearServicio();
        servicio.Anular(id);
        return Ok("Venta anulada correctamente!!!!!");
    }
}