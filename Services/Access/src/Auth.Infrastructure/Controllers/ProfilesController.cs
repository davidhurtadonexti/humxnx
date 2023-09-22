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
public class ProfilesController : ControllerBase
{
    private readonly ILoggerRuntime _logger;
    private readonly ProfilesServiceHandler _profilesServiceHandler;

    public ProfilesController(ILoggerRuntime logger, ProfilesServiceHandler profilesServiceHandler)
    {
        _logger = logger;
        _profilesServiceHandler = profilesServiceHandler;
    }

    /// <summary>
    /// Obtiene todos los registros
    /// </summary>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpGet("GetAll")]
    public ActionResult<List<Profiles>> GetAll()
    {
        try
        {
            var result = _profilesServiceHandler.GetAllProfile();
            var response = new ResponseData<List<Profiles>>
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
    /// <param name="ProfileID">Identificacion del perfil</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [Route("GetOne/{ProfileID}")]
    [HttpGet]
    public ActionResult<ResponseData<Profiles>> GetOne(Guid ProfileID)
    {
        try
        {
            var result = _profilesServiceHandler.GetOneProfile(ProfileID);
            var response = new ResponseData<Profiles>
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
    /// <param name="profiles">Datos del perfil</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPost("NewItem")]
    public ActionResult<ResponseData<Profiles>> NewItem(Profiles profiles)
    {
        try
        {
            var data = new Profiles
            {
                id = profiles.id != null ? profiles.id : Guid.NewGuid(),
                module_id = profiles.module_id,
                profile_name = profiles.profile_name,
                status = profiles.status
            };
            var result = _profilesServiceHandler.CreateProfile(data);
            var response = new ResponseData<Profiles>
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
    /// <param name="ProfileID">ID del perfil</param>
    /// <param name="profiles">Datos del perfil para actualizar</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPut("UpdateItem")]
    public ActionResult<ResponseData<Profiles>> UpdateItem(Guid ProfileID, Profiles profiles)
    {
        try
        {
            var result = _profilesServiceHandler.UpdateProfile(ProfileID, profiles);
            var response = new ResponseData<Profiles>
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
    /// <param name="ProfileID">ID del perfil</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpDelete("DeleteItem")]
    public ActionResult<ResponseData<bool>> DeleteItem(Guid ProfileID)
    {
        try
        {
            var result = _profilesServiceHandler.DeleteProfile(ProfileID);
            var response = new ResponseData<bool>
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
}

