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
public class ModulesController : ControllerBase
{
    private readonly ILoggerRuntime _logger;
    private readonly ModulesServiceHandler _modulesServiceHandler;

    public ModulesController(ILoggerRuntime logger, ModulesServiceHandler modulesServiceHandler)
    {
        _logger = logger;
        _modulesServiceHandler = modulesServiceHandler;
    }
    /// <summary>
    /// Obtiene todos los registros
    /// </summary>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpGet("GetAll")]
    public ActionResult<ResponseData<List<Modules>>> GetAll()
    {
        try
        {
            var result = _modulesServiceHandler.GetAllModule();
            var response = new ResponseData<List<Modules>>
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
    /// <param name="ModuleID">Identificacion del modulo</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [Route("GetOne/{ModuleID}")]
    [HttpGet]
    public ActionResult<ResponseData<Modules>> GetOne(Guid ModuleID)
    {
        try
        {
            var result = _modulesServiceHandler.GetOneModule(ModuleID);
            var response = new ResponseData<Modules>
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
    /// <param name="modules">Datos del modulo</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPost("NewItem")]
    public ActionResult<ResponseData<Modules>> NewItem(Modules modules)
    {
        try
        {
            var data = new Modules
            {
                id = modules.id != null ? modules.id : Guid.NewGuid(),
                enterprise_id = modules.enterprise_id,
                name = modules.name,
                url = modules.url
            };
            var result = _modulesServiceHandler.CreateModule(data);

            var response = new ResponseData<Modules>
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
    /// <param name="ModuleID">ID del modulo</param>
    /// <param name="modules">Datos del modulo para actualizar</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPut("UpdateItem")]
    public ActionResult<ResponseData<Modules>> UpdateItem(Guid ModuleID, Modules modules)
    {
        try
        {
            var result = _modulesServiceHandler.UpdateModule(ModuleID, modules);
            var response = new ResponseData<Modules>
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
    /// <param name="ModuleID">ID del modulo</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpDelete("DeleteItem")]
    public ActionResult<ResponseData<bool>> DeleteItem(Guid ModuleID)
    {
        try
        {
            var result = _modulesServiceHandler.DeleteModule(ModuleID);
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

