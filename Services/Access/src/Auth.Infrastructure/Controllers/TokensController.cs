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
public class TokensController : ControllerBase
{
        private readonly ILoggerRuntime _logger;
        private readonly TokensServiceHandler _tokensServiceHandler;

        public TokensController(ILoggerRuntime logger, TokensServiceHandler tokensServiceHandler)
		{
            _logger = logger;
            _tokensServiceHandler = tokensServiceHandler;
        }

        /// <summary>
        /// Obtiene todos los registros
        /// </summary>
        /// <returns>
        /// ActionResult
        /// </returns>
        [HttpGet("GetAll")]
        public ActionResult<ResponseData<List<Tokens>>> GetAll()
        {
            try
            {
                var result = _tokensServiceHandler.GetAllToken();
                var response = new ResponseData<List<Tokens>>
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
        /// <param name="TokenID">Identificacion del Tokens</param>
        /// <returns>
        /// ActionResult
        /// </returns>
        [Route("GetOne/{TokenID}")]
        [HttpGet]
        public ActionResult<ResponseData<Tokens>> GetOne(Guid TokenID)
        {
            try
            {
                var result = _tokensServiceHandler.GetOneToken(TokenID);
                var response = new ResponseData<Tokens>
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
        /// <param name="Token">Datos del Tokens</param>
        /// <returns>
        /// ActionResult
        /// </returns>
        [HttpPost("NewItem")]
        public ActionResult<ResponseData<Tokens>> NewItem(Tokens Token)
        {
            try
            {
                var data = new Tokens
                {
                    id = Token.id != null ? Token.id : Guid.NewGuid(),
                    access_token = Token.access_token,
                    expiration_token_time = Token.expiration_token_time,
                    refresh_token = Token.refresh_token,
                    expiration_refresh_token_time = Token.expiration_refresh_token_time,
                    status = Token.status
                };
                var result = _tokensServiceHandler.CreateToken(data);
                var response = new ResponseData<Tokens>
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
        /// <param name="TokenID">ID del Tokens</param>
        /// <param name="Token">Datos del Tokens para actualizar</param>
        /// <returns>
        /// ActionResult
        /// </returns>
        [HttpPut("UpdateItem")]
        public ActionResult<ResponseData<Tokens>> UpdateItem(Guid TokenID, Tokens Token)
        {
            try
            {
                var result = _tokensServiceHandler.UpdateToken(TokenID, Token);
                var response = new ResponseData<Tokens>
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
        /// <param name="TokenID">ID del Tokens</param>
        /// <returns>
        /// ActionResult
        /// </returns>
        [HttpDelete("DeleteItem")]
        public ActionResult<ResponseData<bool>> DeleteItem(Guid TokenID)
        {
            try
            {
                var result = _tokensServiceHandler.DeleteToken(TokenID);
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

