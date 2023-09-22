using Dapper;
using Microsoft.Extensions.Configuration;
using OAuth2.OAuth2.Domain.Entities;
using OAuth2.src.OAuth2.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth2.src.OAuth2.Infraestructure.Repositories.DB
{
    public class Connection : IConnection
    {
        static string conn;

        public Connection(IConfiguration configuration)
        {
            conn = configuration.GetConnectionString("OAuthConnectionString");
        }
        public DbConnection connect()
        {
            DbConnection connection = new SqlConnection(conn);
            return connection;
        }

        public async Task<List<T>> GetAll<T>(string SQLQuery)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    await con.OpenAsync();
                    var List = con.Query<T>(SQLQuery).ToList();
                    return List;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> GetOne<T>(string SQLQuery)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                await con.OpenAsync();
                T obj = con.Query<T>(SQLQuery).SingleOrDefault();
                return obj;
            }

        }


        public async Task<Guid> SaveGetIdData(string SQLQuery, List<SqlParameter> parameters)
        {

            using (SqlConnection con = new SqlConnection(conn))
            {
                await con.OpenAsync();

                using (SqlCommand command = new SqlCommand(SQLQuery, con))
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    if (dataTable.Rows.Count > 0)
                    {
                        return Guid.Parse(dataTable.Rows[0][0].ToString());
                    }
                }

            }

            return Guid.Empty;
        }

        public async Task<Resultado> UpdateDataById(string SQLQuery, List<SqlParameter> parameters)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                await con.OpenAsync();

                using (SqlCommand command = new SqlCommand(SQLQuery, con))
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    if (dataTable.Rows.Count > 0)
                    {
                        var Resul = new Resultado();
                        Resul.MensajeControl = dataTable.Rows[0][0].ToString();
                        Resul.Error = Int32.Parse(dataTable.Rows[0][1].ToString());
                        Resul.Respuesta1 = dataTable.Rows[0][2].ToString();
                        Resul.Respuesta2 = dataTable.Rows[0][3].ToString();

                        return Resul;
                    }
                }

            }

            return null;
        }
    }
}