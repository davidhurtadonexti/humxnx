using Microsoft.AspNetCore.Cors.Infrastructure;
using pkg.Interfaces;

namespace pkg.Security
{
	public class CorsBase : ICors
    {

        public CorsPolicy GetCustomCorsPolicy(string Origins, string Headers, string Methods)
        {
            var policy = new CorsPolicy();

            // Permite solicitudes desde cualquier origen (por ejemplo, "*", "http://example.com")
            policy.Origins.Add(Origins);

            // Permite cualquier encabezado en las solicitudes
            policy.Headers.Add(Headers);

            // Permite cualquier método HTTP (GET, POST, PUT, DELETE, etc.)
            policy.Methods.Add(Methods);

            return policy;
        }
        public CorsPolicy GetDefaultCorsPolicy()
        {
            var policy = new CorsPolicy();

            // Permite solicitudes desde cualquier origen (por ejemplo, "*", "http://example.com")
            policy.Origins.Add("*");

            // Permite cualquier encabezado en las solicitudes
            policy.Headers.Add("*");

            // Permite cualquier método HTTP (GET, POST, PUT, DELETE, etc.)
            policy.Methods.Add("*");

            return policy;
        }
    }
}

