using Auth.Application.CaseUses;
using Auth.Domain.Interfaces;
using Auth.Infrastructure.DataBaseContext;
using Auth.Infrastructure.Repository;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using pkg.cypher;
using pkg.Interfaces;
using pkg.Logging;
using pkg.Security;
using pkg.Token;
using System;

namespace Auth.Infrastructure.Mapping
{
    public class MappingInterfaces
	{
        public static void MappingInterfacesAll(IServiceCollection services, IConfiguration configuration)
        {
            //add seguridad
            MappingSecurity(services, configuration);

            // add extras
            MappingExtras(services, configuration);

            //add base de datos
            MappingDatabase(services, configuration);

            //add interfaces
            MappingInterfacesApplication(services, configuration);
            MappingInterface(services, configuration);

            //add integraciones
            MappingIntegration(services, configuration);
        }
        public static void MappingInterfacesApplication(IServiceCollection services, IConfiguration configuration)
        {
            //Add services to the container
           services.AddScoped<IEnterprises, EnterprisesRepository>();
           services.AddScoped<IMenus, MenusRepository>();
           services.AddScoped<IModules, ModulesRepository>();
           services.AddScoped<IProfiles, ProfilesRepository>();
           services.AddScoped<IProtectedData, ProtectedDataRepository>();
           services.AddScoped<IResources, ResourcesRepository>();
           services.AddScoped<IUsers, UsersRepository>();
           services.AddScoped<ILogin, LoginRepository>();
           services.AddScoped<ITokens, TokensRepository> ();

           services.AddScoped<UsersServiceHandler>();
           services.AddScoped<ResourcesServiceHandler>();
           services.AddScoped<ProtectedDataServiceHandler>();
           services.AddScoped<ProfilesServiceHandler>();
           services.AddScoped<ModulesServiceHandler>();
           services.AddScoped<MenusServiceHandler>();
           services.AddScoped<EnterprisesServiceHandler>();
           services.AddScoped<LoginServiceHandler>();
           services.AddScoped<TokensServiceHandler>();

        }
        public static void MappingIntegration(IServiceCollection services, IConfiguration configuration)
        {
            // Configure log4net
            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new System.IO.FileInfo("log4net.config"));

            // Add ILogger service
            services.AddSingleton<ILoggerRuntime, LoggerBase>();

            //Add IToken service
            // Register the implementation of IToken using TokenBase
            services.AddSingleton<IToken>(provider =>
            {
                var secretKey = configuration["TokenJWT:secretKey"];
                var secretKeyEncode = configuration["TokenJWT:secretKeyEncode"];
                var issuer = configuration["TokenJWT:issuer"];
                var audience = configuration["TokenJWT:audience"];
                var expiresType = configuration["TokenJWT:expiresType"];
                int expiresValue = int.Parse(configuration["TokenJWT:expiresValue"]);
                // You can also resolve other dependencies needed by TokenBase here
                return new TokenBase(secretKey, secretKeyEncode, issuer, audience, expiresType, expiresValue);
            });

            //Add ICypher service
            services.AddSingleton<ICypher, CypherBase>();

            //Add ICsrf
            services.AddSingleton<ICsrf, CsrfBase>();
        }
        public static void MappingInterface(IServiceCollection services, IConfiguration Configuration)
        { }
        public static void MappingSecurity(IServiceCollection services, IConfiguration Configuration)
        {
            //add auth CSRF
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN"; // Nombre del encabezado para el token CSRF
            });
        }
        public static void MappingDatabase(IServiceCollection services, IConfiguration Configuration)
        {
            // Register DbContext
            var conection = Configuration.GetConnectionString("DefaultConnection");
            //var logger = LogManager.GetLogger(typeof(Program));
            //logger.Info($"Database String: '{conection}'");
            services.AddDbContext<AuthDbContext>(options => {
                options.UseSqlServer(conection, sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                });
            }, ServiceLifetime.Scoped); // Configura como Scoped

            // Create tables and migrate db scheme
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
                dbContext.Database.EnsureCreated(); // This initializes the database and creates tables
                                                    //dbContext.EnsureMissingTablesCreated();
                                                    //dbContext.Database.Migrate();
            }
        }
        public static void MappingExtras(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddMvc();
            services.AddControllers();
            // Add services to the container.

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                // Include 'SecurityScheme' to use JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                // Include 'SecurityScheme' to use CSRF
                var csrfnewSecurityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "X-CSRF-TOKEN",
                    Description = "Token CSRF para protección contra CSRF",

                    Reference = new OpenApiReference
                    {
                        Id = "X-CSRF-TOKEN",
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                // Agregar manualmente un campo X-CSRF-TOKEN a las solicitudes Swagger
                c.AddSecurityDefinition(csrfnewSecurityScheme.Reference.Id, csrfnewSecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                        { jwtSecurityScheme, Array.Empty<string>() },
                        { csrfnewSecurityScheme, Array.Empty<string>() }
                    });
            });
        }
    }
}

