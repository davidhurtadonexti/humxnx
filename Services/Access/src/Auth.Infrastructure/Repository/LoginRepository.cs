using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Access.Auth.Domain.Entities;
using Access.Auth.Domain.Interfaces;
using Access.Auth.Infrastructure.DataBaseContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using pkg.Exceptions;
using pkg.Interfaces;
using LoginEntity = Access.Auth.Domain.Entities.LoginSP;
namespace Access.Auth.Infrastructure.Repository
{
	public class LoginRepository : ILogin
    {
        private readonly ILoggerRuntime _logger;
        private readonly AuthDbContext _context;
        private DbContextOptions<AuthDbContext> _options;

        public LoginRepository(AuthDbContext context, ILoggerRuntime logger, DbContextOptions<AuthDbContext> options)
        {
            _context = context;
            _logger = logger;
            _options = options;
        }
        public List<LoginEntity> GetUserDataLogin(Login login)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var username = new SqlParameter("@username", login.username);
                    var password = new SqlParameter("@password", login.password);
                    var enterprise_id = new SqlParameter("@enterprise_id", login.enterprise_id);
                    var module_id = new SqlParameter("@module_id", login.module_id);

                    var result = context.LoginSP.FromSqlRaw("LoginSP @username, @password, @enterprise_id, @module_id",
                        username,
                        password,
                        enterprise_id,
                        module_id).ToList();
                    if (result.Count() > 0)
                    {
                        return result;
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
        public List<ProtectedDataSP> GetProfileGrants(Guid token_id)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var token = new SqlParameter("@token_id", token_id);

                    var result = context.ProtectedDataSP.FromSqlRaw("ProtectedDataSP @token_id",
                        token).ToList();
                    if (result.Count() > 0)
                    {
                        return result;
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
        public List<LoginEntity> GetUserDataBytoken(Guid token_id, Guid moduleID)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var token = new SqlParameter("@token_id", token_id);
                    var module = new SqlParameter("@module_id", moduleID);

                    var result = context.LoginSP.FromSqlRaw("LoadGrantsUser @token_id, @module_id",
                        token,
                        module).ToList();
                    if (result.Count() > 0)
                    {
                        return result;
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
    }
}