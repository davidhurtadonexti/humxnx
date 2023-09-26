using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Access.Auth.Application.CaseUses;
using Access.Auth.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using pkg.Attributes;
using pkg.Exceptions;
using pkg.Interfaces;
using static pkg.Attributes.General;

namespace Access.Auth.Infrastructure.Controllers;
[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILoggerRuntime _logger;
    private readonly IToken _tokenService;
    private readonly ICypher _cypher;
    private readonly LoginServiceHandler _loginServiceHandler;
    private readonly UsersServiceHandler _usersServiceHandler;
    private readonly TokensServiceHandler _tokensServiceHandler;
    private readonly ICsrf _csrf;

    public LoginController(
        ILoggerRuntime logger,
        IToken token,
        LoginServiceHandler loginServiceHandler,
        ICypher cypher,
        ICsrf csrf,
        UsersServiceHandler usersServiceHandler,
        TokensServiceHandler tokensServiceHandler)
    {
        _logger = logger;
        _tokenService = token;
        _cypher = cypher;
        _loginServiceHandler = loginServiceHandler;
        _csrf = csrf;
        _usersServiceHandler = usersServiceHandler;
        _tokensServiceHandler = tokensServiceHandler;

    }

    [HttpPost("Access")]
    [ValidateAntiForgeryToken]
    public ActionResult<ResponseData<Token>> Access()
    {
        try
        {
            // Simulación de autenticación exitosa
            var claim = new List<Claim>
                {
                new Claim("ProfileID", "D7DB1CA2-52F4-4FD5-924D-2921D7AF1499"),
                new Claim("ResourceID", "6C404AAB-F98A-4A38-8B22-03C9A181C265"),
                new Claim("Access", "rwudx")
                };
            var token = _tokenService.GetToken(claim);
            var response = new ResponseData<Token>
            {
                message = "Get token",
                data = new Token { accessToken = token }
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

    [HttpPost("CrearTokenByGuid")]
    public ActionResult<ResponseData<Token>> CrearTokenByGuid(string guidToken)
    {
        try
        {
            // Simulación de autenticación exitosa
            var claim = new List<Claim>
                {
                new Claim("token_id", guidToken),
                };
            var token = _tokenService.GetToken(claim);
            var response = new ResponseData<Token>
            {
                message = "Get token",
                data = new Token { accessToken = token }
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

    [HttpGet("LoadGrants")]
    public IActionResult LoadGrants()
    {
        try
        {
            var pass = false;
            var headers = HttpContext.Request.Headers;
            var recursos = new List<LoginSP>();
            // Access specific headers
            if (headers.TryGetValue("Authorization", out var headerValue))
            {
                string cleanedToken = headerValue.FirstOrDefault().Replace("Bearer", "").Trim();
                var validateToken = _tokenService.ValidateToken(cleanedToken);
                if (validateToken != null)
                {
                    var userClaims = validateToken.Claims.Select(c => new { c.Type, c.Value }).ToList();
                    var tokenID = Guid.Parse(userClaims.Find(x => x.Type == "token_id").Value);
                    var resultToken = _tokensServiceHandler.GetOneToken(tokenID);
                    var validateTokenTable = _tokenService.ValidateToken(resultToken.access_token);
                    if (validateTokenTable != null)
                    {
                        var TokenClaims = validateTokenTable.Claims.Select(c => new { c.Type, c.Value }).ToList();
                        var moduleID = Guid.Parse(TokenClaims.Find(x => x.Type == "module_id").Value);
                        recursos = _loginServiceHandler.GetUserDataBytoken(tokenID, moduleID);
                        if (recursos[0].Message == "Ok")
                        {
                            pass = true;
                        }
                    }
                }
            }
            if (pass)
            {
                var response = new ResponseData<List<LoginSP>>
                {
                    message = "Get token",
                    data = recursos
                };
                return Ok(response);
            }
            else
            {
                var response = new ResponseData<List<LoginSP>>
                {
                    message = "Error al generar permisos",
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

    [HttpPost("GenerateToken")]
    public ActionResult<ResponseData<Token>> GenerateToken(Login login)
    {
        try
        {
            var pass = false;
            login.password = _cypher.CalculateSHA512Hash(login.password);
            var UserInfo = _usersServiceHandler.GetOneByUsernameUser(login.username);
            if (UserInfo == null)
            {
                pass = false;
                var response = new ResponseData<Token>
                {
                    message = "Error usuario o contraseña",
                    data = null
                };
                return BadRequest(response);
            }
            var tokenInfo = UserInfo != null ? _tokensServiceHandler.GetOneToken(UserInfo.token_id) : null;
            var resultUser = _loginServiceHandler.LoginUser(login);
            if (resultUser.Count() != 0 && resultUser[0].Message == "Ok")
            {
                //int index = 0;
                //añade todos los registros en los claims
                var claims = new List<Claim>();
                //foreach (var item in resultUser)
                //{
                claims.Add(new Claim($"user_id", UserInfo.id.Value != null ? UserInfo.id.Value.ToString():""));
                claims.Add(new Claim($"module_id", resultUser[0].ModuleID != null ? resultUser[0].ModuleID.Value.ToString() : ""));
                //    index++;
                //}
                if (tokenInfo != null)
                {
                    tokenInfo.status = TokenStatus.inactive;
                    Guid id = tokenInfo.id.Value;
                    _tokensServiceHandler.UpdateToken(id, tokenInfo);
                }
                //genera el token
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
                if (CreateToken != null)
                {
                    UserInfo.token_id = CreateToken.id.Value;
                    var update = _usersServiceHandler.UpdateUser(UserInfo.id.Value, UserInfo);
                    if (update != null)
                        pass = true;
                }
                if (pass)
                {
                    var response = new ResponseData<Token>
                    {
                        message = "Get token",
                        data = new Token { accessToken = CreateToken.id.ToString() }
                    };
                    return Ok(response);
                } else
                {
                    var response = new ResponseData<Token>
                    {
                        message = "Error al generar token",
                        data = null
                    };
                    return BadRequest(response);
                }

            } else
            {
                var response = new ResponseData<Token>
                {
                    message = resultUser.Count() != 0 ? resultUser[0].Message : "Error usuario o contraseña",
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

    [HttpGet("ValidateToken")]
    public ActionResult<ResponseData<bool>> ValidateToken([FromQuery] string token)
    {
        try
        {
            var principal = _tokenService.ValidateToken(token);
            if (principal != null)
            {
                var response = new ResponseData<bool>
                {
                    message = "Get validate",
                    data = true
                };
                return Ok(response);
            }
            else
            {
                var response = new ResponseData<bool>
                {
                    message = "Get validate",
                    data = false
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
    [HttpGet("getCsrfToken")]
    public ActionResult<ResponseData<string>> GetCsrfToken()
    {
        var tokens = _csrf.GenerateCsrfToken(HttpContext);
        var response = new ResponseData<string>
        {
            message = "Get token",
            data = tokens
        };
        return Ok(response);
    }
}

