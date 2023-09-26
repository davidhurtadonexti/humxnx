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
public class ProtectedDataController : ControllerBase
{
    private readonly ILoggerRuntime _logger;
    private readonly ProtectedDataServiceHandler _protectedDataServiceHandler;
    public ProtectedDataController(ILoggerRuntime logger, ProtectedDataServiceHandler protectedDataServiceHandler)
    {
        _logger = logger;
        _protectedDataServiceHandler = protectedDataServiceHandler;
    }

    /// <summary>
    /// Obtiene todos los registros
    /// </summary>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpGet("GetAll")]
    public ActionResult<ResponseData<List<ProtectedData>>> GetAll()
    {
        try
        {
            var result = _protectedDataServiceHandler.GetAllProtectedData();
            var response = new ResponseData<List<ProtectedData>>
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
    /// <param name="ProtectedDataID">Identificacion del Protected Data</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [Route("GetOne/{ProtectedDataID}")]
    [HttpGet]
    public ActionResult<ResponseData<ProtectedData>> GetOne(Guid ProtectedDataID)
    {
        try
        {
            var result = _protectedDataServiceHandler.GetOneProtectedData(ProtectedDataID);
            var response = new ResponseData<ProtectedData>
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
    /// <param name="ProtectedData">Datos del Protected Data</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPost("NewItem")]
    public ActionResult<ResponseData<ProtectedData>> NewItem(ProtectedData ProtectedData)
    {
        try
        {
            var data = new ProtectedData
            {
                id = ProtectedData.id != null ? ProtectedData.id : Guid.NewGuid(),
                tabla_name = ProtectedData.tabla_name,
                fields = ProtectedData.fields
            };
            var result = _protectedDataServiceHandler.CreateProtectedData(data);

            var response = new ResponseData<ProtectedData>
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
    /// <param name="ProtectedDataID">ID del Protected Data</param>
    /// <param name="ProtectedData">Datos del Protected Data para actualizar</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPut("UpdateItem")]
    public ActionResult<ResponseData<ProtectedData>> UpdateItem(Guid ProtectedDataID, ProtectedData ProtectedData)
    {
        try
        {
            var result = _protectedDataServiceHandler.UpdateProtectedData(ProtectedDataID, ProtectedData);
            var response = new ResponseData<ProtectedData>
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
    /// <param name="ProtectedDataID">ID del Protected Data</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpDelete("DeleteItem")]
    public ActionResult<ResponseData<bool>> DeleteItem(Guid ProtectedDataID)
    {
        try
        {
            var result = _protectedDataServiceHandler.DeleteProtectedData(ProtectedDataID);
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
