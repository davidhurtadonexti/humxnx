using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using pkg.Security;
using System.Linq;

namespace Gateway.Infrastructure.Mapping
{
    public class MappingPolicies
    {
        //private readonly ICors _corsService;

        //public MappingPolicies (ICors corsService) {
        //    _corsService = corsService;
        //}

        public static void MappingSecurityPolicies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options => {
                var _corsService = new CorsBase();
                options.AddPolicy("CORSPolicy", builder => {
                    var corsPolicy = _corsService.GetCustomCorsPolicy(configuration["CorsPolicy:Origins"],
                        configuration["CorsPolicy:Headers"],
                        configuration["CorsPolicy:Methods"]);
                    builder.WithOrigins(corsPolicy.Origins.ToArray())
                           .WithHeaders(corsPolicy.Headers.ToArray())
                           .WithMethods(corsPolicy.Methods.ToArray());
                });
            });
        }
    }
}