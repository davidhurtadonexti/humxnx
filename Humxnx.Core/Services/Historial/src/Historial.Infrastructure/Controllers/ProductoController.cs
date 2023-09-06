using System;
using System.Collections.Generic;
using Humxnx.Historial.Core.Application.Services;
using Humxnx.Historial.Core.Domain.Entities;
using Humxnx.Historial.Core.Infrastructure.Repositories.DB.Contexts;
using Humxnx.Historial.Core.Infrastructure.Repositories.DB.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Humxnx.Historial.Core.Infrastructure.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductoController : ControllerBase
{
    private ProductoServicio CrearServicio()
    {
        var db = new VentaContexto();
        var repo = new ProductoRepositorio(db);
        var servicio = new ProductoServicio(repo);
        return servicio;
    }

    // GET: api/<ProductoController>
    [HttpGet]
    public ActionResult<List<Producto>> Get()
    {
        var servicio = CrearServicio();
        return Ok(servicio.Listar());
    }

    // GET api/<ProductoController>/5
    [HttpGet("{id}")]
    public ActionResult<Producto> Get(Guid id)
    {
        var servicio = CrearServicio();
        
        var products = servicio.Listar();
        var p = products.Find(x => x.productoId == id);
        if (p == null)
            return new NotFoundResult();
        // servicio.SeleccionarPorID(id)
            
        return Ok(p);
    }

    // POST api/<ProductoController>
    [HttpPost]
    public ActionResult Post([FromBody] Producto producto)
    {
        var servicio = CrearServicio();
        servicio.Agregar(producto);
        return Ok("Agregar satisfactoriamente!!!!!");
    }

    // PUT api/<ProductoController>/5
    [HttpPut("{id}")]
    public ActionResult Put(Guid id, [FromBody] Producto producto)
    {
        var servicio = CrearServicio();
        producto.productoId = id;
        servicio.Editar(producto);
        return Ok("Editado debidamente!!!!!!!!!");
    }

    // DELETE api/<ProductoController>/5
    [HttpDelete("{id}")]
    public ActionResult Delete(Guid id)
    {
        var servicio = CrearServicio();
        servicio.Eliminar(id);
        return Ok("Eliminado correctamente!!!!!");
    }
}