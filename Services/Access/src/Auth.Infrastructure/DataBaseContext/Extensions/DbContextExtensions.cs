using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Access.Auth.Infrastructure.DataBaseContext.Extensions
{
    internal static class DbContextExtensions
    {

        internal static void EnsureCreatingMissingTables<TDbContext>(this TDbContext dbContext) where TDbContext : DbContext
        {
            var type = typeof(TDbContext);
            var dbSetType = typeof(DbSet<>);

            var dbPropertyNames = type.GetProperties().Where(p => p.PropertyType.Name == dbSetType.Name)
                .Select(p => p.Name).ToArray();

            foreach (var entityName in dbPropertyNames)
            {
                CheckTableExistsAndCreateIfMissing(dbContext, entityName);
            }
        }

        private static void CheckTableExistsAndCreateIfMissing(DbContext dbContext, string entityName)
        {
            var defaultSchema = dbContext.Model.GetDefaultSchema();
            var tableName = string.IsNullOrWhiteSpace(defaultSchema) ? $"[{entityName}]" : $"[{defaultSchema}].[{entityName}]";

            try
            {
                _ = dbContext.Database.ExecuteSqlRaw($"SELECT TOP(1) * FROM {tableName}"); //Throws on missing table
            }
            catch (Exception)
            {
                var scriptStart = $"CREATE TABLE {tableName}";
                const string scriptEnd = "GO";
                var script = dbContext.Database.GenerateCreateScript();

                var tableScript = script.Split(scriptStart).Last().Split(scriptEnd);
                var first = $"{scriptStart} {tableScript.First()}";

                dbContext.Database.ExecuteSqlRaw(first);
                // var logger = LogManager.GetLogger(typeof(Program));
                // logger.Info($"Database table: '{tableName}' was created.");
            }
        }
    }
}

