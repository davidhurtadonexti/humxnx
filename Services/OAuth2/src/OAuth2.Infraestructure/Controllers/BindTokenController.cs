using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OAuth2.OAuth2.Domain.Entities;
using OAuth2.src.OAuth2.Application.Interfaces;
using OAuth2.src.OAuth2.Infraestructure.Contracts.BindToken;
using OAuth2.src.OAuth2.Infraestructure.Contracts.RefreshToken;
namespace OAuth2.src.OAuth2.Infraestructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BindTokenController : ControllerBase
    {
        public IBindToken _bind;
        public ITokenGenerator _tokenGenerator;
        public IRefreshToken _refresh;
        public IValidateToken _validation;

        public BindTokenController(IBindToken bind, ITokenGenerator tokenGenerator, IRefreshToken refresh, IValidateToken validation)
        {
            _bind = bind;
            _refresh = refresh;
            _tokenGenerator = tokenGenerator;
            _validation = validation;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BindTokenInput input, HttpRequest request)
        {
            string responseToken = "";
            try { 
                string authorizationToken = request.Headers["Authorization"];
                if (authorizationToken == null) throw new Exception("Authorization Token obligatorio");


                //*********************************** validar TOKEN ************************************
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(authorizationToken); JwtSecurityToken tokenS = jsonToken as JwtSecurityToken;
                string authorizationId = tokenS.Claims.First(claim => claim.Type == "AuthorizationToken").Value;
                string accessToken = tokenS.Claims.First(claim => claim.Type == "AccessToken").Value;

                //******************************** valida que no esté lleno el token **********************
                if(Guid.Parse(accessToken) != Guid.Empty || Guid.Parse(input.AccessToken) == Guid.Empty)
                {
                        throw new Exception("No es posible bindear el token");
                }
            
                using (TransactionScope scope = new(TransactionScopeOption.RequiresNew, TransactionScopeAsyncFlowOption.Enabled))
                {
                    TokenDataReturn tokenData = await _validation.GetDataToken(authorizationId);
                    if (tokenData == null) throw new Exception("No existe token.");

                    //****************************** validar que se puede hacer refresh ******************************
                    jsonToken = handler.ReadToken(tokenData.AuthorizationToken);
                    tokenS = jsonToken as JwtSecurityToken;

                    TokenClaimDto dataAuthorizationToken = JsonConvert.DeserializeObject<TokenClaimDto>(tokenS.Claims.First(claim => claim.Type == "TokenData").Value);
                    
                    DateTime tokenExpiresIn = tokenData.wasUpdated ? tokenData.UpdatedAt.AddMinutes(dataAuthorizationToken.AccesTokenExp) : tokenData.CreatedAt.AddMinutes(dataAuthorizationToken.AccesTokenExp);

                    DateTime refreshExpiresIn = tokenData.CreatedAt.AddMinutes(dataAuthorizationToken.RefreshTokenExp);

                    if (DateTime.UtcNow > refreshExpiresIn) throw new Exception("Refresh Token expirado");

                    //****************************** generar nuevo refresh ******************************
                    AuthorizationTokenClaimDto newResponseToken = new AuthorizationTokenClaimDto
                    {
                        AuthorizationToken = Guid.Parse(authorizationId),
                        AccessToken = Guid.Parse(input.AccessToken)
                    };

                    string newToken = _tokenGenerator.GenerateToken(JsonConvert.SerializeObject(dataAuthorizationToken), "RefreshWithClaims", dataAuthorizationToken.AccesTokenExp);

                    //****************************** Actualizar registro en bd ******************************
                    Resultado updatedResponse = await _refresh.UpdateToken(newToken, Guid.Parse(authorizationId));

                    if (updatedResponse.Error != 0) throw new Exception("Error al actualizar Token");

                    scope.Complete();

                    responseToken = _tokenGenerator.GenerateToken(JsonConvert.SerializeObject(newResponseToken), "RefreshAuthorization", dataAuthorizationToken.AccesTokenExp);
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