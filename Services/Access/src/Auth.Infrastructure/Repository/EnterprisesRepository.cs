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

namespace Access.Auth.Infrastructure.Repository
{
	public class EnterprisesRepository : IEnterprises
    {
        private readonly AuthDbContext _context;
        private readonly ILoggerRuntime _logger;
        private DbContextOptions<AuthDbContext> _options;
        public EnterprisesRepository(AuthDbContext context, ILoggerRuntime logger, DbContextOptions<AuthDbContext> options)
		{
            _context = context;
            _logger = logger;
            _options = options;
        }
        public List<Enterprises> GetAll()
		{
            return _context.Enterprises.ToListAsync().Result;
        }
        public Enterprises GetOne(Guid EnterpriseID)
        {
            return _context.Enterprises.FindAsync(EnterpriseID).Result;
        }
        public Enterprises NewItem(Enterprises enterprise)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Enterprises.FromSqlRaw("EXEC CrudEnterprise @Accion, @id, @name, @status, @identification",
                        new SqlParameter("@Accion", 1),
                        new SqlParameter("@id", enterprise.id),
                        new SqlParameter("@name", enterprise.name),
                        new SqlParameter("@status", enterprise.status),
                        new SqlParameter("@identification", enterprise.identification)
                            ).ToList();
                    if (result.Count() > 0)
                    {
                        return enterprise;
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
        public Enterprises UpdateItem(Guid EnterpriseID, Enterprises enterprise)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Enterprises.FromSqlRaw("EXEC CrudEnterprise @Accion, @id, @name, @status, @identification",
                        new SqlParameter("@Accion", 2),
                        new SqlParameter("@id", EnterpriseID),
                        new SqlParameter("@name", enterprise.name),
                        new SqlParameter("@status", enterprise.status),
                        new SqlParameter("@identification", enterprise.identification)
                            ).ToList();
                    if (result.Count() > 0)
                    {
                        return enterprise;
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
        public bool DeleteItem(Guid EnterpriseID)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Enterprises.FromSqlRaw("EXEC CrudEnterprise @Accion, @id",
                        new SqlParameter("@Accion", 3),
                        new SqlParameter("@id", EnterpriseID)
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