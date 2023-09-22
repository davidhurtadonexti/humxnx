using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace pkg.Interfaces
{
	public interface IToken
	{
        public string GetToken(List<Claim> claims);
        public ClaimsPrincipal ValidateToken(string token);
        public Guid GenerateRefreshToken();
        public bool ValidateRefreshToken(string refreshToken);
        public bool VerifyRefreshToken(string refreshToken, string storedRefreshToken);
    }
}

