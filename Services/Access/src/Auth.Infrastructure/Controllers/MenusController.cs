using System;
using System.Collections.Generic;
using System.Diagnostics;
using Access.Auth.Application.CaseUses;
using Access.Auth.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using pkg.Exceptions;
using pkg.Interfaces;
using static pkg.Attributes.General;

namespace Access.Auth.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenusController : ControllerBase
{
    private readonly ILoggerRuntime _logger;
    private readonly MenusServiceHandler _menusServiceHandler;

   public MenusController(ILoggerRuntime logger, MenusServiceHandler menusServiceHandler)
    {
        _logger = logger;
        _menusServiceHandler = menusServiceHandler;
    }

    /// <summary>
    /// Obtiene todos los registros
    /// </summary>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpGet("GetAll")]
    public ActionResult<ResponseData<List<Menus>>> GetAll()
    {
        try
        {
            var result = _menusServiceHandler.GetAllMenu();
            var response = new ResponseData<List<Menus>>
            {
                message = "Get all data",
                data = result
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            // Create a StackTrace object
            var stackTrace = new StackTrace(ex, true);

            // Create and throw the enhanced exception
            throw new ExceptionStackTrace("An error occurred.", ex, stackTrace, _logger);
        }
    }
    /// <summary>
    /// Obtiene un documento
    /// </summary>
    /// <param name="MenuID">Identificacion del menu</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [Route("GetOne/{MenuID}")]
    [HttpGet]
    public ActionResult<Menus> GetOne(Guid MenuID)
    {
        try
        {
            var result = _menusServiceHandler.GetOneMenu(MenuID);
            var response = new ResponseData<Menus>
            {
                message = "Get one data",
                data = result
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            // Create a StackTrace object
            var stackTrace = new StackTrace(ex, true);

            // Create and throw the enhanced exception
            throw new ExceptionStackTrace("An error occurred.", ex, stackTrace, _logger);
        }
    }
    /// <summary>
    /// Crea un documento
    /// </summary>
    /// <param name="menus">Datos del menu</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPost("NewItem")]
    public ActionResult<Menus> NewItem(Menus menus)
    {
        try
        {
            var data = new Menus
            {
                id = menus.id != null ? menus.id : Guid.NewGuid(),
                parent_id = menus.parent_id,
                module_id = menus.module_id,
                name = menus.name,
                url = menus.url,
                order = menus.order,
                status = menus.status
            };
            var result = _menusServiceHandler.CreateMenu(data);

            var response = new ResponseData<Menus>
            {
                message = "Save one data",
                data = result
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            // Create a StackTrace object
            var stackTrace = new StackTrace(ex, true);

            // Create and throw the enhanced exception
            throw new ExceptionStackTrace("An error occurred.", ex, stackTrace, _logger);
        }
    }
    /// <summary>
    /// Actualiza un documento
    /// </summary>
    /// <param name="MenuID">ID del menu</param>
    /// <param name="menus">Datos del menu para actualizar</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPut("UpdateItem")]
    public ActionResult<ResponseData<Menus>> UpdateItem(Guid MenuID, Menus menus)
    {
        try
        {
            var result = _menusServiceHandler.UpdateMenu(MenuID, menus);
            var response = new ResponseData<Menus>
            {
                message = "Update one data",
                data = result
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            // Create a StackTrace object
            var stackTrace = new StackTrace(ex, true);

            // Create and throw the enhanced exception
            throw new ExceptionStackTrace("An error occurred.", ex, stackTrace, _logger);
        }
    }
    /// <summary>
    /// Eliminado logico un documento
    /// </summary>
    /// <param name="MenuID">ID del menu</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpDelete("DeleteItem")]
    public ActionResult<ResponseData<bool>> DeleteItem(Guid MenuID)
    {
        try
        {
            var result = _menusServiceHandler.DeleteMenu(MenuID);
            var response = new ResponseData<bool>
            {
                message = "Delete one data",
                data = result
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            // Create a StackTrace object
            var stackTrace = new StackTrace(ex, true);

            // Create and throw the enhanced exception
            throw new ExceptionStackTrace("An error occurred.", ex, stackTrace, _logger);
        }
    }
}

