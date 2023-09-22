using System.Data.SqlClient;
using System.Data;
using OAuth2.src.OAuth2.Application.Interfaces;
using OAuth2.src.OAuth2.Domain.Interfaces;
using OAuth2.src.OAuth2.Infraestructure.Contracts.RefreshToken;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using OAuth2.OAuth2.Domain.Entities;

namespace OAuth2.src.OAuth2.Application.Services
{
    public class RefreshTokenService : IRefreshToken
    {
        IConnection _connection;
        public RefreshTokenService(IConnection connection)
        {
            _connection = connection;
        }

        public async Task<TokenDataReturn> GetDataToken(string authorizationId, string refreshToken)
        {
            return await _connection.GetOne<TokenDataReturn>($@"sp_get_token_data_by_refresh '{authorizationId}', '{refreshToken}'");
        }

        public async Task<Resultado> UpdateToken(string token, Guid id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@tokenId", id));
            parametros.Add(new SqlParameter("@newToken", token));

            SqlParameter pMensaje = new SqlParameter("@mensajeControl", SqlDbType.VarChar, 400);
            pMensaje.Direction = ParameterDirection.Output;
            parametros.Add(pMensaje);

            SqlParameter pError = new SqlParameter("@error", SqlDbType.Decimal, 50);
            pError.Direction = ParameterDirection.Output;
            parametros.Add(pError);

            SqlParameter pRespuesta1 = new SqlParameter("@respuesta1", SqlDbType.VarChar, 400);
            pRespuesta1.Direction = ParameterDirection.Output;
            parametros.Add(pRespuesta1);

            SqlParameter pRespuesta2 = new SqlParameter("@respuesta2", SqlDbType.VarChar, 400);
            pRespuesta2.Direction = ParameterDirection.Output;
            parametros.Add(pRespuesta2);

            return await _connection.UpdateDataById("sp_update_token", parametros);
        }
    }
}
