
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using OAuth2.OAuth2.Domain.Entities;

namespace OAuth2.src.OAuth2.Domain.Interfaces
{
    public interface IConnection
    {
        Task<T> GetOne<T>(string SQLQuery);

        Task<List<T>> GetAll<T>(string SQLQuery);

        Task<Guid> SaveGetIdData(string SQLQuery, List<SqlParameter> parameters);

        Task<Resultado> UpdateDataById(string SQLQuery, List<SqlParameter> parameters);
    }
}
