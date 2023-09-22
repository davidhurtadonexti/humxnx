using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OAuth2.src.OAuth2.Infraestructure.Contracts.GetToken;

namespace OAuth2.src.OAuth2.Application.Interfaces
{
    public interface IAuthorizationToken
    {
        public Task<Guid> SaveToken(Guid id, string token, string refreshToken);
        public Task<List<ClaimDataReturn>> GetTokenData(string clientId, string secretKey);
    }
}
