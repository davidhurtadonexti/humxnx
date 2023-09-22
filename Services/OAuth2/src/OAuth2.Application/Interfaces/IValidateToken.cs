using System;
using System.Threading.Tasks;
using OAuth2.OAuth2.Domain.Entities;
using OAuth2.src.OAuth2.Infraestructure.Contracts.RefreshToken;

namespace OAuth2.src.OAuth2.Application.Interfaces
{
    public interface IValidateToken
    {
        public Task<TokenDataReturn> GetDataToken(string authorizationId);
        public Task<Resultado> UnauthorizeToken(Guid id);
    }
}
