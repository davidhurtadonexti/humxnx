using System;
using System.Threading.Tasks;
using OAuth2.OAuth2.Domain.Entities;
using OAuth2.src.OAuth2.Infraestructure.Contracts.RefreshToken;

namespace OAuth2.src.OAuth2.Application.Interfaces
{
    public interface IRefreshToken
    {
        public Task<Resultado> UpdateToken(string token, Guid id);
        public Task<TokenDataReturn> GetDataToken(string authorizationId, string refreshToken);
    }
}
