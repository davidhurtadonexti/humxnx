using System;
using Microsoft.AspNetCore.Http;

namespace pkg.Interfaces
{
	public interface ICsrf
	{
        public string GenerateCsrfToken(HttpContext httpContext);

    }
}

