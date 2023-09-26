using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using Access.Auth.Application.CaseUses;
using Access.Auth.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using pkg.Exceptions;
using pkg.Interfaces;
using static pkg.Attributes.General;

namespace Access.Auth.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILoggerRuntime _logger;
    private readonly ICypher _cypher;
    private readonly UsersServiceHandler _usersServiceHandler;
    private readonly IToken _tokenService;
    private readonly TokensServiceHandler _tokensServiceHandler;

    public UsersController(ILoggerRuntime logger, ICypher cypher, UsersServiceHandler usersServiceHandler, IToken token, TokensServiceHandler tokensServiceHandler)
    {
        _logger = logger;
        _cypher = cypher;
        _usersServiceHandler = usersServiceHandler;
        _tokenService = token;
        _tokensServiceHandler = tokensServiceHandler;
    }

    /// <summary>
    /// Obtiene todos los registros
    /// </summary>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpGet("GetAll")]
    public ActionResult<ResponseData<List<Users>>> GetAll()
    {
        try
        {
            var result = _usersServiceHandler.GetAllUser();
            var response = new ResponseData<List<Users>>
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
    /// <param name="UsersID">Identificacion del Users</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [Route("GetOne/{UsersID}")]
    [HttpGet]
    public ActionResult<ResponseData<Users>> GetOne(Guid UsersID)
    {
        try
        {
            var result = _usersServiceHandler.GetOneUser(UsersID);
            var response = new ResponseData<Users>
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
    /// <param name="Users">Datos del Users</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPost("NewItem")]
    public ActionResult<ResponseData<Users>> NewItem(Users Users)
    {
        try
        {
            var pass = false;
            //int index = 0;
            //añade todos los registros en los claims
            var claims = new List<Claim>();
            //foreach (var item in resultUser)
            //{
            claims.Add(new Claim($"enterprise_id", Users.enterprise_id != null ? Users.enterprise_id.ToString() : ""));
            var token = _tokenService.GetToken(claims);
            //var validateToken = _tokenService.ValidateToken(token);
            var SchemeToken = new Tokens
            {
                id = Guid.NewGuid(),
                access_token = token,
                expiration_token_time = "",
                refresh_token = _tokenService.GenerateRefreshToken().ToString(),
                expiration_refresh_token_time = "",
                status = TokenStatus.active

            };
            // crea el token basado en los claims
            // nota: validar si el guid se lo presenta como unico en el campo de refresh token
            var CreateToken = _tokensServiceHandler.CreateToken(SchemeToken);
            var Password = _cypher.CalculateSHA512Hash(Users.Protected);
            Users UserData = null;
            if (CreateToken != null)
            {
                var data = new Users
                {
                    id = Users.id != null ? Users.id : Guid.NewGuid(),
                    enterprise_id = Users.enterprise_id,
                    username = Users.username,
                    Protected = Password,
                    logged = Users.logged,
                    token_id = CreateToken.id.Value
                };
                var result = _usersServiceHandler.CreateUser(data);
                if (result != null)
                    pass = true;
                    UserData = result;
            }

            if (pass)
            {
                var response = new ResponseData<Users>
                {
                    message = "Save one data",
                    data = UserData
                };
                return Ok(response);
            }
            else
            {
                var response = new ResponseData<Users>
                {
                    message = "Error to create user",
                    data = null
                };
                return BadRequest(response);
            }
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
    /// <param name="UsersID">ID del Users</param>
    /// <param name="Users">Datos del Users para actualizar</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPut("UpdateItem")]
    public ActionResult<ResponseData<Users>> UpdateItem(Guid UsersID, Users Users)
    {
        try
        {
            var result = _usersServiceHandler.UpdateUser(UsersID, Users);
            var response = new ResponseData<Users>
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
    /// <param name="UsersID">ID del Users</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpDelete("DeleteItem")]
    public ActionResult<ResponseData<bool>> DeleteItem(Guid UsersID)
    {
        try
        {
            var result = _usersServiceHandler.DeleteUser(UsersID);

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
    /// <summary>
    /// Vincular Profile
    /// </summary>
    /// <param name="Users_Profiles">Datos Profiles y Resources</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPost("Link/Profile")]
    public ActionResult<ResponseData<Users_Profiles>> LinkProfile(Users_Profiles Users_Profiles)
    {
        try
        {
            var data = new Users_Profiles
            {
                id = Users_Profiles.id != null ? Users_Profiles.id : Guid.NewGuid(),
                user_id = Users_Profiles.user_id,
                profile_id = Users_Profiles.profile_id
            };
            var result = _usersServiceHandler.LinkProfile(data);
            var response = new ResponseData<Users_Profiles>
            {
                message = "Save one link data",
                data = data
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
    /// Vincular ProtectData
    /// </summary>
    /// <param name="Profile_ProtectedData">Datos Profile y ProtectedData</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPost("Link/ProtectData")]
    public ActionResult<ResponseData<Profile_ProtectedData>> LinkProtectData(Profile_ProtectedData Profile_ProtectedData)
    {
        try
        {
            var data = new Profile_ProtectedData
            {
                id = Profile_ProtectedData.id != null ? Profile_ProtectedData.id : Guid.NewGuid(),
                protected_id = Profile_ProtectedData.protected_id,
                profile_id = Profile_ProtectedData.profile_id
            };
            var result = _usersServiceHandler.LinkProtectData(data);
            var response = new ResponseData<Profile_ProtectedData>
            {
                message = "Save one link data",
                data = data
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
    /// Vincular Resources
    /// </summary>
    /// <param name="Profiles_Resources">Datos Profiles y Recursos</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPost("Link/Resources")]
    public ActionResult<ResponseData<Profiles_Resources>> LinkResources(Profiles_Resources Profiles_Resources)
    {
        try
        {
            var data = new Profiles_Resources
            {
                id = Profiles_Resources.id != null ? Profiles_Resources.id : Guid.NewGuid(),
                profile_id = Profiles_Resources.profile_id,
                resources_id = Profiles_Resources.resources_id,
                access = Profiles_Resources.access
            };
            var result = _usersServiceHandler.LinkResources(data);
            var response = new ResponseData<Profiles_Resources>
            {
                message = "Save one link data",
                data = data
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
    /// Update Profile
    /// </summary>
    /// <param name="ItemID">ID a actualizar</param>
    /// <param name="Users_Profiles">Datos Profiles y Resources</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPut("Link/Profile")]
    public ActionResult<ResponseData<Users_Profiles>> LinkProfileUpdate(Guid ItemID, Users_Profiles Users_Profiles)
    {
        try
        {
            var result = _usersServiceHandler.LinkProfileUpdate(ItemID, Users_Profiles);
            var response = new ResponseData<Users_Profiles>
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
    /// Update ProtectData
    /// </summary>
    /// <param name="ItemID">ID a actualizar</param>
    /// <param name="Profile_ProtectedData">Datos Profile y ProtectedData</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPut("Link/ProtectData")]
    public ActionResult<ResponseData<Profile_ProtectedData>> LinkProtectDataUpdate(Guid ItemID, Profile_ProtectedData Profile_ProtectedData)
    {
        try
        {
            var result = _usersServiceHandler.LinkProtectDataUpdate(ItemID, Profile_ProtectedData);
            var response = new ResponseData<Profile_ProtectedData>
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
    /// Update Resources
    /// </summary>
    /// <param name="ItemID">ID a actualizar</param>
    /// <param name="Profiles_Resources">Datos Profiles y Recursos</param>
    /// <returns>
    /// ActionResult
    /// </returns>
    [HttpPut("Link/Resources")]
    public ActionResult<ResponseData<Profiles_Resources>> LinkResourcesUpdate(Guid ItemID, Profiles_Resources Profiles_Resources)
    {
        try
        {
            var result = _usersServiceHandler.LinkResourcesUpdate(ItemID, Profiles_Resources);
            var response = new ResponseData<Profiles_Resources>
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


