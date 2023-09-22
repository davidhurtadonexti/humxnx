using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using pkg.Interfaces;
using pkg.Logging;
using pkg.Security;
using pkg.Token;
using System;
using System.IO;
using System.Text;


namespace Gateway.Infrastructure.Mapping
{
    public class MappingInterfaces
	{
        public static void MappingInterfacesAll(IServiceCollection services, ConfigurationManager configuration)
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
        { }
        public static void MappingIntegration(IServiceCollection services, IConfiguration Configuration)
        {
            // Configure log4net
            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            //catch all console
            var logFile = Path.Combine(Directory.GetCurrentDirectory(), "console.log");
            var logWriter = new StreamWriter(logFile, append: true) { AutoFlush = true };

            Console.SetOut(logWriter);
            Console.SetError(logWriter);

            // Add ILogger service
            services.AddSingleton<ILoggerRuntime, LoggerBase>();

            //Add IToken service
            // Register the implementation of IToken using TokenBase
            services.AddSingleton<IToken>(provider =>
            {
                var secretKey = Configuration["TokenJWT:secretKey"];
                var secretKeyEncode = Configuration["TokenJWT:secretKeyEncode"];
                var issuer = Configuration["TokenJWT:issuer"];
                var audience = Configuration["TokenJWT:audience"];
                var expiresType = Configuration["TokenJWT:expiresType"];
                int expiresValue = int.Parse(Configuration["TokenJWT:expiresValue"]);
                // You can also resolve other dependencies needed by TokenBase here
                return new TokenBase(secretKey, secretKeyEncode, issuer, audience, expiresType, expiresValue);
            });
            //Add ICors
            services.AddSingleton<ICors, CorsBase>();
        }
        public static void MappingInterface(IServiceCollection services, IConfiguration Configuration)
        { }
        public static void MappingSecurity(IServiceCollection services, IConfiguration Configuration)
        {
            //add policies
            MappingPolicies.MappingSecurityPolicies(services, Configuration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidIssuer = "apiIssuer",
                    ValidateAudience = false,
                    ValidAudience = "apiAudience",
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero,
                    TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenJWT:secretKeyEncode"])),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenJWT:secretKey"])),
                };
                options.RequireHttpsMetadata = Convert.ToBoolean(Configuration["AuthServer:RequireHttpsMetadata"]);
                options.SaveToken = true;
                //options.Audience = "apiAudience";
            });
        }
        public static void MappingDatabase(IServiceCollection services, IConfiguration Configuration)
        { }
        public static void MappingExtras(IServiceCollection services, Microsoft.Extensions.Configuration.ConfigurationManager Configuration)
        {
            Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

            //services.AddMvc();
            services.AddControllers();

            services.AddCors();
            services.AddOcelot();

            services.AddSwaggerGen(null);
            /*services.AddSwaggerForOcelot(Configuration, (obj) =>
            {
                obj.GenerateDocsDocsForGatewayItSelf(c =>
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

                    c.GatewayDocsTitle = "Gateway";
                    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                    // Agregar manualmente un campo X-CSRF-TOKEN a las solicitudes Swagger
                    c.AddSecurityDefinition(csrfnewSecurityScheme.Reference.Id, csrfnewSecurityScheme);

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                        { jwtSecurityScheme, Array.Empty<string>() },
                        { csrfnewSecurityScheme, Array.Empty<string>() }
                    });
                });
            });*/
        }
    }
}

