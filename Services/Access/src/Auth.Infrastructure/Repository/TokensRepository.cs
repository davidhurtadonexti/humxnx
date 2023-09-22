using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Auth.Domain.Entities;
using Auth.Domain.Interfaces;
using Auth.Infrastructure.DataBaseContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using pkg.Exceptions;
using pkg.Interfaces;

namespace Auth.Infrastructure.Repository
{
	public class TokensRepository : ITokens
    {
        private readonly AuthDbContext _context;
        private readonly ILoggerRuntime _logger;
        private DbContextOptions<AuthDbContext> _options;
        public TokensRepository(AuthDbContext context, ILoggerRuntime logger, DbContextOptions<AuthDbContext> options)
        {
            _context = context;
            _logger = logger;
            _options = options;
        }
        public List<Tokens> GetAll()
        {
            return _context.Tokens.ToListAsync().Result;
        }
        public Tokens GetOne(Guid TokenID)
        {
            return _context.Tokens.FindAsync(TokenID).Result;
        }
        public Tokens NewItem(Tokens token)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Tokens.FromSqlRaw("EXEC CrudToken @Accion, @id, @accessToken, @expirationTokenTime, @refreshToken, @expirationRefreshTokenTime, @status",
                        new SqlParameter("@Accion", 1),
                        new SqlParameter("@id", token.id),
                        new SqlParameter("@accessToken", token.access_token),
                        new SqlParameter { ParameterName = "@expirationTokenTime", SqlDbType = SqlDbType.NVarChar, Value = token.expiration_token_time },
                        new SqlParameter { ParameterName = "@refreshToken", SqlDbType = SqlDbType.NVarChar, Value = token.refresh_token },
                        new SqlParameter { ParameterName = "@expirationRefreshTokenTime", SqlDbType = SqlDbType.NVarChar, Value = token.expiration_refresh_token_time },
                        new SqlParameter("@status", token.status)
                        ).ToList();
                    if (result.Count() > 0)
                    {
                        return token;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Create a StackTrace object
                var stackTrace = new StackTrace(ex, true);

                // Create and throw the enhanced exception
                throw new ExceptionStackTrace("An error occurred.", ex, stackTrace, _logger);
            }
        }
        public Tokens UpdateItem(Guid TokenID, Tokens token)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Tokens.FromSqlRaw("EXEC CrudToken @Accion, @id, @accessToken, @expirationTokenTime, @refreshToken, @expirationRefreshTokenTime, @status",
                        new SqlParameter("@Accion", 2),
                        new SqlParameter("@id", TokenID),
                        new SqlParameter("@accessToken", token.access_token),
                        new SqlParameter { ParameterName = "@expirationTokenTime", SqlDbType = SqlDbType.NVarChar, Value = token.expiration_token_time },
                        new SqlParameter { ParameterName = "@refreshToken", SqlDbType = SqlDbType.NVarChar, Value = token.refresh_token },
                        new SqlParameter { ParameterName = "@expirationRefreshTokenTime", SqlDbType = SqlDbType.NVarChar, Value = token.expiration_refresh_token_time},
                        new SqlParameter("@status", token.status)
                        ).ToList();
                    if (result.Count() > 0)
                    {
                        return token;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Create a StackTrace object
                var stackTrace = new StackTrace(ex, true);

                // Create and throw the enhanced exception
                throw new ExceptionStackTrace("An error occurred.", ex, stackTrace, _logger);
            }
        }
        public bool DeleteItem(Guid TokenID)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Tokens.FromSqlRaw("EXEC CrudToken @Accion, @id",
                        new SqlParameter("@Accion", 3),
                        new SqlParameter("@id", TokenID)
                        ).ToList();
                    if (result.Count() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Create a StackTrace object
                var stackTrace = new StackTrace(ex, true);

                // Create and throw the enhanced exception
                throw new ExceptionStackTrace("An error occurred.", ex, stackTrace, _logger);
            }
        }
    }
}

