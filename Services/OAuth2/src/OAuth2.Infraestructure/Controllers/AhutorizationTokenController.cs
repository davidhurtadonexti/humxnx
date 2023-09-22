using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Transactions;
using OAuth2.src.OAuth2.Application.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using OAuth2.src.OAuth2.Infraestructure.Contracts.GetToken;

namespace OAuth2.src.OAuth2.Infraestructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationTokenController : ControllerBase
    {
        public IAuthorizationToken _auth;
        public ITokenGenerator _tokenGenerator;

        public AuthorizationTokenController(IAuthorizationToken auth, ITokenGenerator tokenGenerator)
        {
            _auth = auth;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GetTokenInput input)
        {
            GetTokenResponse response = new GetTokenResponse()
            {
                AuthorizationToken = "",
                RefreshToken = ""
            };

            using (TransactionScope scope = new(TransactionScopeOption.RequiresNew, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    List<ClaimDataReturn> claimData = await _auth.GetTokenData(input.ClientId, input.SecretKey);

                    if (claimData.Count < 1) throw new Exception("Cliente no encontrado");

                    var token = _tokenGenerator.GenerateToken(JsonConvert.SerializeObject(claimData), "Claims", claimData[0].AccesTokenExp);

                    var refreshToken = _tokenGenerator.GenerateToken("", "Refresh", claimData[0].RefreshTokenExp);

                    Guid tokenId = await _auth.SaveToken(claimData[0].Id, token, refreshToken);

                    if (tokenId == Guid.Empty) throw new Exception("Ocurrió un problema al Generar Token");

                    var responseToken = _tokenGenerator.GenerateToken(tokenId.ToString(), "Authorization", claimData[0].AccesTokenExp);

                    response = new GetTokenResponse()
                    {
                        AuthorizationToken = responseToken,
                        RefreshToken = refreshToken,
                        ExpirationToken = claimData[0].AccesTokenExp
                    };
                    scope.Complete();
                }
                catch(Exception ex)
                {
                    return Unauthorized();
                }
            }

            return Ok(response);
        }
    }
}
