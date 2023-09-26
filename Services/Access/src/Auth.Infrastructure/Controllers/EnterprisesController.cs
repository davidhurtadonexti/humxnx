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
public class EnterprisesController : ControllerBase
{
    private readonly ILoggerRuntime _logger;
    private readonly EnterprisesServiceHandler _enterprisesServiceHandler;

    public EnterprisesController(ILoggerRuntime logger, EnterprisesServiceHandler enterprisesServiceHandler)
    {
        _logger = logger;
        _enterprisesServiceHandler = enterprisesServiceHandler;
    }

    /// <summary>
    /// Obtiene todos los registros
    /// </summary>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpGet("GetAll")]
    public ActionResult<ResponseData<List<Enterprises>>> GetAll()
    {
        try
        {
            var result = _enterprisesServiceHandler.GetAllEnterprise();
            var response = new ResponseData<List<Enterprises>>
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
    /// <param name="EnterpriseID">Identificacion de la empresa</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [Route("GetOne/{EnterpriseID}")]
    [HttpGet]
    public ActionResult<ResponseData<Enterprises>> GetOne(Guid EnterpriseID)
    {
        try
        {
            var result = _enterprisesServiceHandler.GetOneEnterprise(EnterpriseID);
            var response = new ResponseData<Enterprises>
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
    /// <param name="enterprises">Datos de la empresa</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPost("NewItem")]
    public ActionResult<ResponseData<Enterprises>> NewItem(Enterprises enterprises)
    {
        try
        {
            var data = new Enterprises
            {
                id = enterprises.id != null ? enterprises.id : Guid.NewGuid(),
                name = enterprises.name,
                status = enterprises.status,
                identification = enterprises.identification
            };
            var result = _enterprisesServiceHandler.CreateEnterprise(data);

            var response = new ResponseData<Enterprises>
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
    /// <param name="EnterpriseID">ID de la empresa</param>
    /// <param name="enterprises">Datos de la empresa para actualizar</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPut("UpdateItem")]
    public ActionResult<ResponseData<Enterprises>> UpdateItem(Guid EnterpriseID, Enterprises enterprises)
    {
        try
        {
            var result = _enterprisesServiceHandler.UpdateEnterprise(EnterpriseID, enterprises);
            var response = new ResponseData<Enterprises>
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
    /// <param name="EnterpriseID">ID de la empresa</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpDelete("DeleteItem")]
    public ActionResult<ResponseData<bool>> DeleteItem(Guid EnterpriseID)
    {
        try
        {
            var result = _enterprisesServiceHandler.DeleteEnterprise(EnterpriseID);
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

