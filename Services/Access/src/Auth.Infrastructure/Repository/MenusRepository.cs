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
	public class MenusRepository : IMenus
	{
        private readonly AuthDbContext _context;
        private readonly ILoggerRuntime _logger;
        private DbContextOptions<AuthDbContext> _options;
        public MenusRepository(AuthDbContext context, ILoggerRuntime logger, DbContextOptions<AuthDbContext> options)
        {
            _context = context;
            _logger = logger;
            _options = options;
        }
        public List<Menus> GetAll()
        {
            return _context.Menus.ToListAsync().Result;
        }
        public Menus GetOne(Guid MenuID)
        {
            return _context.Menus.FindAsync(MenuID).Result;
        }
        public Menus NewItem(Menus Menu)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Menus.FromSqlRaw("EXEC CrudMenu @Accion, @id, @parent_id, @module_id, @name, @order, @url, @status",
                        new SqlParameter("@Accion", 1),
                        new SqlParameter("@id", Menu.id),
                        new SqlParameter("@parent_id", Menu.parent_id),
                        new SqlParameter("@module_id", Menu.module_id),
                        new SqlParameter("@name", Menu.name),
                        new SqlParameter("@order", Menu.order),
                        new SqlParameter("@url", Menu.url),
                        new SqlParameter("@status", Menu.status)
                            ).ToList();
                    if (result.Count() > 0)
                    {
                        return Menu;
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
        public Menus UpdateItem(Guid MenuID, Menus Menu)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Menus.FromSqlRaw("EXEC CrudMenu @Accion, @id, @parent_id, @module_id, @name, @order, @url, @status",
                        new SqlParameter("@Accion", 2),
                        new SqlParameter("@id", MenuID),
                        new SqlParameter("@parent_id", Menu.parent_id),
                        new SqlParameter("@module_id", Menu.module_id),
                        new SqlParameter("@name", Menu.name),
                        new SqlParameter("@order", Menu.order),
                        new SqlParameter("@url", Menu.url),
                        new SqlParameter("@status", Menu.status)
                            ).ToList();
                    if (result.Count() > 0)
                    {
                        return Menu;
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
        public bool DeleteItem(Guid MenuID)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Menus.FromSqlRaw("EXEC CrudMenu @Accion, @id",
                        new SqlParameter("@Accion", 3),
                        new SqlParameter("@id", MenuID)
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