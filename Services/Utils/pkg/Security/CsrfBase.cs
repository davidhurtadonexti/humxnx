using System;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using pkg.Interfaces;

namespace pkg.Security
{
	public class CsrfBase : ICsrf
    {
        private readonly IAntiforgery _antiforgery;

        public CsrfBase(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public string GenerateCsrfToken(HttpContext httpContext)
        {
            // Genera un token CSRF personalizado utilizando _antiforgery
            var tokens = _antiforgery.GetAndStoreTokens(httpContext);
            return tokens.RequestToken;
        }
    }
}

