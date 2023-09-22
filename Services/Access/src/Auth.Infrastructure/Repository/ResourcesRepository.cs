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
	public class ResourcesRepository : IResources
    {
        private readonly AuthDbContext _context;
        private readonly ILoggerRuntime _logger;
        private DbContextOptions<AuthDbContext> _options;
        public ResourcesRepository(AuthDbContext context, ILoggerRuntime logger, DbContextOptions<AuthDbContext> options)
        {
            _context = context;
            _logger = logger;
            _options = options;
        }
        public List<Resources> GetAll()
        {
            return _context.Resources.ToListAsync().Result;
        }
        public Resources GetOne(Guid ResourceID)
        {
            return _context.Resources.FindAsync(ResourceID).Result;
        }
        public Resources NewItem(Resources resource)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Resources.FromSqlRaw("EXEC CrudResource @Accion, @id, @menu_id, @resource",
                        new SqlParameter("@Accion", 1),
                        new SqlParameter("@id", resource.id),
                        new SqlParameter("@menu_id", resource.menu_id),
                        new SqlParameter("@resource", resource.resource)
                            ).ToList();
                    if (result.Count() > 0)
                    {
                        return resource;
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
        public Resources UpdateItem(Guid ResourceID, Resources resource)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Resources.FromSqlRaw("EXEC CrudResource @Accion, @id, @menu_id, @resource",
                       new SqlParameter("@Accion", 2),
                       new SqlParameter("@id", ResourceID),
                       new SqlParameter("@menu_id", resource.menu_id),
                       new SqlParameter("@resource", resource.resource)
                           ).ToList();
                    if (result.Count() > 0)
                    {
                        return resource;
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
        public bool DeleteItem(Guid ResourceID)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Resources.FromSqlRaw("EXEC CrudResource @Accion, @id",
                       new SqlParameter("@Accion", 3),
                       new SqlParameter("@id", ResourceID)
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

