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
	public class UsersRepository : IUsers
	{
        private readonly ILoggerRuntime _logger;
        private readonly AuthDbContext _context;
        private DbContextOptions<AuthDbContext> _options;
        public UsersRepository(AuthDbContext context, ILoggerRuntime logger, DbContextOptions<AuthDbContext> options)
        {
            _context = context;
            _logger = logger;
            _options = options;
        }
        public List<Users> GetAll()
        {
            return _context.Users.ToListAsync().Result;
        }
        public Users GetOne(Guid UserID)
        {
            return _context.Users.FindAsync(UserID).Result;
        }
        public Users GetOneByUserName(string username)
        {
            try
            {
                return _context.Users.FirstOrDefault(u => u.username == username);
            }
            catch (Exception ex)
            {
                // Create a StackTrace object
                var stackTrace = new StackTrace(ex, true);

                // Create and throw the enhanced exception
                throw new ExceptionStackTrace("An error occurred.", ex, stackTrace, _logger);
            }
        }
        public Users NewItem(Users user)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Users.FromSqlRaw("EXEC CrudUser @Accion, @id, @enterprise_id, @username, @protected, @token_id, @logged",
                    new SqlParameter("@Accion", 1),
                    new SqlParameter("@id", user.id),
                        new SqlParameter("@enterprise_id", user.enterprise_id),
                        new SqlParameter("@username", user.username),
                        new SqlParameter("@protected", user.Protected),
                        new SqlParameter("@token_id", user.token_id),
                        new SqlParameter("@logged", user.logged)
                        ).ToList();
                    return result[0];
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
        public Users UpdateItem(Guid UserID, Users user)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Users.FromSqlRaw("EXEC CrudUser @Accion, @id, @enterprise_id, @username, @protected, @token_id, @logged",
                        new SqlParameter("@Accion", 2),
                        new SqlParameter("@id", UserID),
                        new SqlParameter("@enterprise_id", user.enterprise_id),
                        new SqlParameter("@username", user.username),
                        new SqlParameter("@protected", user.Protected),
                        new SqlParameter("@token_id", user.token_id),
                        new SqlParameter("@logged", user.logged)
                            ).ToList();
                    if (result.Count() > 0)
                    {
                        return user;
                    } else
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
        public bool DeleteItem(Guid UserID)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Users.FromSqlRaw("EXEC CrudUser @Accion, @id",
                        new SqlParameter("@Accion", 3),
                        new SqlParameter("@id", UserID)
                            ).ToList();
                    if (result.Count() > 0)
                    {
                        return true;
                    } else
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
        public Profiles_Resources LinkResources(Profiles_Resources Profiles_Resources)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Profiles_Resources.FromSqlRaw("EXEC LinkResources @Accion, @id, @profile_id, @resources_id, @access",
                        new SqlParameter("@Accion", 1),
                        new SqlParameter("@id", Profiles_Resources.id),
                        new SqlParameter("@profile_id", Profiles_Resources.profile_id),
                        new SqlParameter("@resources_id", Profiles_Resources.resources_id),
                        new SqlParameter("@access", Profiles_Resources.access)
                        ).ToList();
                    return result[0];
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
        public Profile_ProtectedData LinkProtectData(Profile_ProtectedData Profile_ProtectedData)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Profile_ProtectedData.FromSqlRaw("EXEC LinkProtectData @Accion, @id, @protected_id, @profile_id",
                        new SqlParameter("@Accion", 1),
                        new SqlParameter("@id", Profile_ProtectedData.id),
                        new SqlParameter("@protected_id", Profile_ProtectedData.protected_id),
                        new SqlParameter("@profile_id", Profile_ProtectedData.profile_id)
                        ).ToList();
                    return result[0];
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
        public Users_Profiles LinkProfile(Users_Profiles Users_Profiles)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Users_Profiles.FromSqlRaw("EXEC LinkProfile @Accion, @id, @user_id, @profile_id",
                        new SqlParameter("@Accion", 1),
                        new SqlParameter("@id", Users_Profiles.id),
                        new SqlParameter("@user_id", Users_Profiles.user_id),
                        new SqlParameter("@profile_id", Users_Profiles.profile_id)
                        ).ToList();
                    return result[0];
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
        public Profiles_Resources LinkResourcesUpdate(Guid ItemID, Profiles_Resources Profiles_Resources)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Profiles_Resources.FromSqlRaw("EXEC LinkResources @Accion, @id, @profile_id, @resources_id, @access",
                        new SqlParameter("@Accion", 2),
                        new SqlParameter("@id", ItemID),
                        new SqlParameter("@profile_id", Profiles_Resources.profile_id),
                        new SqlParameter("@resources_id", Profiles_Resources.resources_id),
                        new SqlParameter("@access", Profiles_Resources.access)
                        ).ToList();
                    return result[0];
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
        public Profile_ProtectedData LinkProtectDataUpdate(Guid ItemID, Profile_ProtectedData Profile_ProtectedData)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Profile_ProtectedData.FromSqlRaw("EXEC LinkProtectData @Accion, @id, @protected_id, @profile_id",
                        new SqlParameter("@Accion", 2),
                        new SqlParameter("@id", ItemID),
                        new SqlParameter("@protected_id", Profile_ProtectedData.protected_id),
                        new SqlParameter("@profile_id", Profile_ProtectedData.profile_id)
                        ).ToList();
                    return result[0];
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
        public Users_Profiles LinkProfileUpdate(Guid ItemID, Users_Profiles Users_Profiles)
        {
            try
            {
                using (var context = new AuthDbContext(_options))
                {
                    var result = context.Users_Profiles.FromSqlRaw("EXEC LinkProfile @Accion, @id, @user_id, @profile_id",
                        new SqlParameter("@Accion", 2),
                        new SqlParameter("@id", ItemID),
                        new SqlParameter("@user_id", Users_Profiles.user_id),
                        new SqlParameter("@profile_id", Users_Profiles.profile_id)
                        ).ToList();
                    return result[0];
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