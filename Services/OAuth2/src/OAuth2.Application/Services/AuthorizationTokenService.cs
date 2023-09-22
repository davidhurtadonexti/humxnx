using OAuth2.src.OAuth2.Application.Interfaces;
using OAuth2.src.OAuth2.Domain.Interfaces;
using OAuth2.src.OAuth2.Infraestructure.Contracts.GetToken;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace OAuth2.src.OAuth2.Application.Services
{
    public class AuthorizationTokenService : IAuthorizationToken
    {
        IConnection _connection;
        public AuthorizationTokenService(IConnection connection) {
            _connection = connection;
        }

        public async Task<List<ClaimDataReturn>> GetTokenData(string clientId, string secretKey)
        {
            return await _connection.GetAll<ClaimDataReturn>($@"sp_get_data_client_claim '{clientId}', '{secretKey}'");
        }

        public async Task<Guid> SaveToken(Guid id, string token, string refreshToken)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@clientId", id));
            parametros.Add(new SqlParameter("@token", token));
            parametros.Add(new SqlParameter("@refreshToken", refreshToken));

            return await _connection.SaveGetIdData("sp_save_authorization_token", parametros);
        }
    }
}
