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
	public class ModulesRepository : IModules
    {
        private readonly AuthDbContext _context;
        private readonly ILoggerRuntime _logger;
        private DbContextOptions<AuthDbContext> _options;
        public ModulesRepository(AuthDbContext context, ILoggerRuntime logger, DbContextOptions<AuthDbContext> options)
        {
            _context = context;
            _logger = logger;
            _options = options;
        }
        public List<Modules> GetAll()
        {
            return _context.Modules.ToListAsync().Result;
        }
        public Modules GetOne(Guid ModuleID)
        {
            return _context.Modules.FindAsync(ModuleID).Result;
        }
        public Modules NewItem(Modules module)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Modules.FromSqlRaw("EXEC CrudModule @Accion, @id, @enterprise_id, @name, @url",
                        new SqlParameter("@Accion", 1),
                        new SqlParameter("@id", module.id),
                        new SqlParameter("@enterprise_id", module.enterprise_id),
                        new SqlParameter("@name", module.name),
                        new SqlParameter("@url", module.url)
                            ).ToList();
                    if (result.Count() > 0)
                    {
                        return module;
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
        public Modules UpdateItem(Guid ModuleID, Modules module)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Modules.FromSqlRaw("EXEC CrudModule @Accion, @id, @enterprise_id, @name, @url",
                        new SqlParameter("@Accion", 2),
                        new SqlParameter("@id", ModuleID),
                        new SqlParameter("@enterprise_id", module.enterprise_id),
                        new SqlParameter("@name", module.name),
                        new SqlParameter("@url", module.url)
                            ).ToList();
                    if (result.Count() > 0)
                    {
                        return module;
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
        public bool DeleteItem(Guid ModuleID)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Modules.FromSqlRaw("EXEC CrudModule @Accion, @id",
                        new SqlParameter("@Accion", 3),
                        new SqlParameter("@id", ModuleID)
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