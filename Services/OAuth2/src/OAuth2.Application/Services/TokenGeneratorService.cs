using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OAuth2.OAuth2.Domain.Entities;
using OAuth2.src.OAuth2.Application.Interfaces;
using OAuth2.src.OAuth2.Infraestructure.Contracts.GetToken;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class TokenGeneratorService: ITokenGenerator
{
    private readonly IConfiguration _config;

    public TokenGeneratorService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(string input, string type, int expiration)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = "";

        switch (type)
        {
            case "Claims":
                //Primera generación del token para la BD
                var claimData = JsonConvert.DeserializeObject<List<ClaimDataReturn>>(input);

                TokenClaimDto data = new()
                {
                    ClientServiceId = claimData[0].Id,
                    ClientDescripcion = claimData[0].ClientDescription,
                    ServiceDescription = claimData[0].ServiceDescription,
                    AccesTokenExp = claimData[0].AccesTokenExp,
                    RefreshTokenExp = claimData[0].RefreshTokenExp,
                    Resources = claimData.Select(z => new PermisionsDto
                    {
                        ResourceDescription = z.ResourceDescription,
                        ResourceUrl = z.ResourceUrl,
                        Scopes = z.Scopes
                    }).ToArray()
                };

                var claims = new[]
                {
                    new Claim("TokenData", JsonConvert.SerializeObject(data))
                };

                token = new JwtSecurityTokenHandler().WriteToken(
                    new JwtSecurityToken(null,
                    null,
                    claims,
                    expires: DateTime.Now.AddMinutes(expiration),
                    signingCredentials: credentials)
                    );
                break;

            case "Refresh":
                //Primera generación del REFRESH
                var randomNumber = new byte[64];
                var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);
                token = Convert.ToBase64String(randomNumber);
                break;

            case "RefreshWithClaims":
                //Refrescar el token para la BD
                var refreshClaims = new[]
                {
                    new Claim("TokenData", input)
                };

                token = new JwtSecurityTokenHandler().WriteToken(
                    new JwtSecurityToken(null,
                    null,
                    refreshClaims,
                    expires: DateTime.Now.AddMinutes(expiration),
                    signingCredentials: credentials)
                    );
                break;

            case "RefreshAuthorization":
                //Refresca el Token para devolver
                AuthorizationTokenClaimDto refreshToken = JsonConvert.DeserializeObject<AuthorizationTokenClaimDto>(input); ;

                var refreshAuthorizationClaims = new[]
                {
                        new Claim("AuthorizationToken", refreshToken.AuthorizationToken.ToString()),
                        new Claim("AccessToken", refreshToken.AccessToken.ToString())
                    };

                token = new JwtSecurityTokenHandler().WriteToken(
                   new JwtSecurityToken(null, null,
                   refreshAuthorizationClaims,
                   expires: DateTime.Now.AddMinutes(expiration),
                   signingCredentials: credentials)
                   );
                break;

            case "Authorization":
                //Primera generación del Token para devolver
                AuthorizationTokenClaimDto responseToken = new AuthorizationTokenClaimDto
                {
                    AuthorizationToken = Guid.Parse(input),
                    AccessToken = Guid.Empty
                };

                var tokenClaim = new[]
                {
                        new Claim("AuthorizationToken", responseToken.AuthorizationToken.ToString()),
                        new Claim("AccessToken", responseToken.AccessToken.ToString())
                    };

                token = new JwtSecurityTokenHandler().WriteToken(
                   new JwtSecurityToken(null, null,
                   tokenClaim,
                   expires: DateTime.Now.AddMinutes(expiration),
                   signingCredentials: credentials)
                   );
                break;
        }


        return token;
    }
}
