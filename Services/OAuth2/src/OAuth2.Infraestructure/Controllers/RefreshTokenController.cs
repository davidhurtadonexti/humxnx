using System;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OAuth2.OAuth2.Domain.Entities;
using OAuth2.src.OAuth2.Application.Interfaces;
using OAuth2.src.OAuth2.Infraestructure.Contracts.RefreshToken;

namespace OAuth2.src.OAuth2.Infraestructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokenController : ControllerBase
    {
        public IRefreshToken _refresh;
        public ITokenGenerator _tokenGenerator;

        public RefreshTokenController(IRefreshToken refresh, ITokenGenerator tokenGenerator)
        {
            _refresh = refresh;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(RefreshTokenInput input, HttpRequest request)
        {
            string responseToken = "";

            try
            {
                using (TransactionScope scope = new(TransactionScopeOption.RequiresNew, TransactionScopeAsyncFlowOption.Enabled))
                {
                    string authorizationToken = request.Headers["Authorization"];
                    
                    if (authorizationToken == null) throw new Exception("Authorization Token obligatorio");

                    //desencriptar el token
                    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(authorizationToken);
                    JwtSecurityToken tokenS = jsonToken as JwtSecurityToken;
    
                    string authorizationId = tokenS.Claims.First(claim => claim.Type == "AuthorizationToken").Value;
                    string accessToken = tokenS.Claims.First(claim => claim.Type == "AccessToken").Value;

                    //validar existencia en el token
                    TokenDataReturn tokenData = await _refresh.GetDataToken(authorizationId, input.RefreshToken);
    
                    if (tokenData == null) throw new Exception("No existe token.");
    
                    jsonToken = handler.ReadToken(tokenData.AuthorizationToken);
                    tokenS = jsonToken as JwtSecurityToken;
    
                    //validar validar tiempo de token
                    //validar tiempo de refresh
    
                    TokenClaimDto dataAuthorizationToken = JsonConvert.DeserializeObject<TokenClaimDto>(tokenS.Claims.First(claim => claim.Type == "TokenData").Value);
    
                    DateTime tokenExpiresIn = tokenData.wasUpdated ? tokenData.UpdatedAt.AddMinutes(dataAuthorizationToken.AccesTokenExp) : tokenData.CreatedAt.AddMinutes(dataAuthorizationToken.AccesTokenExp);
    
                    DateTime refreshExpiresIn = tokenData.CreatedAt.AddMinutes(dataAuthorizationToken.RefreshTokenExp);
    
                    if (DateTime.UtcNow > refreshExpiresIn) throw new Exception("Refresh Token expirado");
    
                    if (DateTime.UtcNow > tokenExpiresIn)
                    {
                        AuthorizationTokenClaimDto newResponseToken = new AuthorizationTokenClaimDto
                        {
                            AuthorizationToken = Guid.Parse(authorizationId),
                            AccessToken = Guid.Parse(accessToken)
                        };
                                
                        string newToken = _tokenGenerator.GenerateToken(JsonConvert.SerializeObject(dataAuthorizationToken), "RefreshWithClaims", dataAuthorizationToken.AccesTokenExp);
                            
                        Resultado updatedResponse = await _refresh.UpdateToken(newToken, Guid.Parse(authorizationId));

                        if(updatedResponse.Error != 0) throw new Exception("Error al actualizar Token");

                        scope.Complete();
    
                        return Ok(_tokenGenerator.GenerateToken(JsonConvert.SerializeObject(newResponseToken), "RefreshAuthorization", dataAuthorizationToken.AccesTokenExp));
                    }
                    else
                    {
                        responseToken = authorizationToken;
                    }
                }
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }

            return Ok(responseToken);
        }
    }
}