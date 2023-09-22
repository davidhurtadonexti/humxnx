using System;
using System.Collections.Generic;
using System.Diagnostics;
using Auth.Application.CaseUses;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using pkg.Exceptions;
using pkg.Interfaces;
using static pkg.Attributes.General;

namespace Auth.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResourcesController : ControllerBase
{
    private readonly ILoggerRuntime _logger;
    private readonly ResourcesServiceHandler _resourcesServiceHandler;

    public ResourcesController(ILoggerRuntime logger, ResourcesServiceHandler resourcesServiceHandler)
    {
        _logger = logger;
        _resourcesServiceHandler = resourcesServiceHandler;
    }

    /// <summary>
    /// Obtiene todos los registros
    /// </summary>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpGet("GetAll")]
    public ActionResult<ResponseData<List<Resources>>> GetAll()
    {
        try
        {
            var result = _resourcesServiceHandler.GetAllResource();
            var response = new ResponseData<List<Resources>>
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
    /// <param name="ResourcesID">Identificacion del Resources</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [Route("GetOne/{ResourcesID}")]
    [HttpGet]
    public ActionResult<ResponseData<Resources>> GetOne(Guid ResourcesID)
    {
        try
        {
            var result = _resourcesServiceHandler.GetOneResource(ResourcesID);
            var response = new ResponseData<Resources>
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
    /// <param name="Resources">Datos del Resources</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPost("NewItem")]
    public ActionResult<ResponseData<Resources>> NewItem(Resources Resources)
    {
        try
        {
            var data = new Resources
            {
                id = Resources.id != null ? Resources.id : Guid.NewGuid(),
                menu_id = Resources.menu_id,
                resource = Resources.resource
            };
            var result = _resourcesServiceHandler.CreateResource(data);
            var response = new ResponseData<Resources>
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
    /// <param name="ResourcesID">ID del Resources</param>
    /// <param name="Resources">Datos del Resources para actualizar</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPut("UpdateItem")]
    public ActionResult<ResponseData<Resources>> UpdateItem(Guid ResourcesID, Resources Resources)
    {
        try
        {
            var result = _resourcesServiceHandler.UpdateResource(ResourcesID, Resources);
            var response = new ResponseData<Resources>
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
    /// <param name="ResourcesID">ID del Resources</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpDelete("DeleteItem")]
    public ActionResult<ResponseData<bool>> DeleteItem(Guid ResourcesID)
    {
        try
        {
            var result = _resourcesServiceHandler.DeleteResource(ResourcesID);
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
