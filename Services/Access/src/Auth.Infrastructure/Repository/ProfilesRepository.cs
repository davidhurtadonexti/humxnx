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
	public class ProfilesRepository : IProfiles
    {
        private readonly AuthDbContext _context;
        private readonly ILoggerRuntime _logger;
        private DbContextOptions<AuthDbContext> _options;
        public ProfilesRepository(AuthDbContext context, ILoggerRuntime logger, DbContextOptions<AuthDbContext> options)
        {
            _context = context;
            _logger = logger;
            _options = options;
        }
        public List<Profiles> GetAll()
        {
            return _context.Profiles.ToListAsync().Result;
        }
        public Profiles GetOne(Guid ProfileID)
        {
            return _context.Profiles.FindAsync(ProfileID).Result;
        }
        public Profiles NewItem(Profiles profile)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Profiles.FromSqlRaw("EXEC CrudProfile @Accion, @id, @module_id, @profile_name, @status",
                        new SqlParameter("@Accion", 1),
                        new SqlParameter("@id", profile.id),
                        new SqlParameter("@module_id", profile.module_id),
                        new SqlParameter("@profile_name", profile.profile_name),
                        new SqlParameter("@status", profile.status)
                            ).ToList();
                    if (result.Count() > 0)
                    {
                        return profile;
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
        public Profiles UpdateItem(Guid ProfileID, Profiles profile)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Profiles.FromSqlRaw("EXEC CrudProfile @Accion, @id, @module_id, @profile_name, @status",
                        new SqlParameter("@Accion", 2),
                        new SqlParameter("@id", ProfileID),
                        new SqlParameter("@module_id", profile.module_id),
                        new SqlParameter("@profile_name", profile.profile_name),
                        new SqlParameter("@status", profile.status)
                            ).ToList();
                    if (result.Count() > 0)
                    {
                        return profile;
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
        public bool DeleteItem(Guid ProfileID)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Profiles.FromSqlRaw("EXEC CrudProfile @Accion, @id",
                        new SqlParameter("@Accion", 3),
                        new SqlParameter("@id", ProfileID)
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

