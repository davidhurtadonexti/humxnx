using System;
using System.Collections.Generic;
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
	public class ProtectedDataRepository : IProtectedData
    {
        private readonly AuthDbContext _context;
        private readonly ILoggerRuntime _logger;
        private DbContextOptions<AuthDbContext> _options;
        public ProtectedDataRepository(AuthDbContext context, ILoggerRuntime logger, DbContextOptions<AuthDbContext> options)
        {
            _context = context;
            _logger = logger;
            _options = options;
        }
        public List<ProtectedData> GetAll()
        {
            return _context.ProtectedData.ToListAsync().Result;
        }
        public ProtectedData GetOne(Guid ProtectedDataID)
        {
            return _context.ProtectedData.FindAsync(ProtectedDataID).Result;
        }
        public ProtectedData NewItem(ProtectedData ProtectedData)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.ProtectedData.FromSqlRaw("EXEC CrudProtectedData @Accion, @id, @tabla_name, @fields",
                        new SqlParameter("@Accion", 1),
                        new SqlParameter("@id", ProtectedData.id),
                        new SqlParameter("@tabla_name", ProtectedData.tabla_name),
                        new SqlParameter("@fields", ProtectedData.fields)
                            ).ToList();
                    if (result.Count() > 0)
                    {
                        return ProtectedData;
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
        public ProtectedData UpdateItem(Guid ProtectedDataID, ProtectedData ProtectedData)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.ProtectedData.FromSqlRaw("EXEC CrudProtectedData @Accion, @id, @tabla_name, @fields",
                        new SqlParameter("@Accion", 2),
                        new SqlParameter("@id", ProtectedDataID),
                        new SqlParameter("@tabla_name", ProtectedData.tabla_name),
                        new SqlParameter("@fields", ProtectedData.fields)
                            ).ToList();
                    if (result.Count() > 0)
                    {
                        return ProtectedData;
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
        public bool DeleteItem(Guid ProtectedDataID)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.ProtectedData.FromSqlRaw("EXEC CrudProtectedData @Accion, @id",
                        new SqlParameter("@Accion", 3),
                        new SqlParameter("@id", ProtectedDataID)
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

