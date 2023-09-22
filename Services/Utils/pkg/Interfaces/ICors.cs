using Microsoft.AspNetCore.Cors.Infrastructure;

namespace pkg.Interfaces
{
	public interface ICors
	{
        public CorsPolicy GetCustomCorsPolicy(string Origins, string Headers, string Methods);
        public CorsPolicy GetDefaultCorsPolicy();
    }
}

