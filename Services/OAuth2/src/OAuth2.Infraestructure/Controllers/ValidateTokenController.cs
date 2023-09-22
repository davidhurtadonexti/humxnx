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
using OAuth2.src.OAuth2.Infraestructure.Contracts.RefreshToken;
using OAuth2.src.OAuth2.Infraestructure.Contracts.ValidateToken;

namespace OAuth2.OAuth2.Infraestructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateTokenController : ControllerBase
    {
        private readonly IValidateToken _validate;
        public ITokenGenerator _tokenGenerator;

        public ValidateTokenController(IValidateToken validate, ITokenGenerator tokenGenerator)
        {
            _validate = validate;
            _tokenGenerator = tokenGenerator;
        }

        public ValidateTokenController()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ValidateTokenInput input, HttpRequest request)
        {
            try
            {
                string authorizationToken = request.Headers["Authorization"];
                if (authorizationToken == null)
                {
                    throw new Exception("Authorization Token obligatorio");
                }


                //*********************************** validar TOKEN ************************************
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(authorizationToken);JwtSecurityToken tokenS = jsonToken as JwtSecurityToken;
                string authorizationId = tokenS.Claims.First(claim => claim.Type == "AuthorizationToken").Value;
                string accessToken = tokenS.Claims.First(claim => claim.Type == "AccessToken").Value;


                //***************************** validar EXSISTENCIA del token ***************************
                TokenDataReturn tokenData = await _validate.GetDataToken(authorizationId);
                if (tokenData == null) 
                { 
                    throw new Exception("No existe token."); 
                }


                //***************************** validar EXPIRACION del token ****************************
                jsonToken = handler.ReadToken(tokenData.AuthorizationToken);
                tokenS = jsonToken as JwtSecurityToken;
                TokenClaimDto dataAuthorizationToken = JsonConvert.DeserializeObject<TokenClaimDto>(tokenS.Claims.First(claim => claim.Type == "TokenData").Value);
                DateTime tokenExpiresIn = tokenData.wasUpdated ? tokenData.UpdatedAt.AddMinutes(dataAuthorizationToken.AccesTokenExp) : tokenData.CreatedAt.AddMinutes(dataAuthorizationToken.AccesTokenExp);
                DateTime refreshExpiresIn = tokenData.CreatedAt.AddMinutes(dataAuthorizationToken.RefreshTokenExp);

                if (DateTime.UtcNow > tokenExpiresIn) {
                    if(DateTime.UtcNow > refreshExpiresIn) { 
                        using (TransactionScope scope = new(TransactionScopeOption.RequiresNew, TransactionScopeAsyncFlowOption.Enabled))
                        {
                            Resultado tokenDeleted = await _validate.UnauthorizeToken(Guid.Parse(authorizationId));

                            if (tokenDeleted.Error != 0) throw new Exception("Error al actualizar Token");

                            scope.Complete();
                        }
                    }
                    throw new Exception("Token Expirado"); 
                }

                //****************************** validar PERMISOS del token *****************************
                if (dataAuthorizationToken.Resources.FirstOrDefault(x => x.ResourceUrl == input.AccessUri) != null)
                {
                    if(dataAuthorizationToken.Resources.FirstOrDefault(x => x.ResourceUrl == input.AccessUri && x.Scopes == input.Method) == null) throw new Exception("Método inválido");
                }
                else
                {
                    throw new Exception("Recurso inválido");
                }
            }
            catch (Exception ex)
            {
                //log
                return Unauthorized();
            }

            return Ok();
        }

    }
}
