using Microsoft.IdentityModel.Tokens;
using pkg.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace pkg.Token
{
    public class TokenBase : IToken
    {
        private readonly string _secretKey;
        private readonly string _secretKeyEncode;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _expiresType;
        private readonly int _expiresValue;

        public TokenBase(string secretKey, string secretKeyEncode, string issuer, string audience, string expiresType, int expiresValue)
		{
            _secretKey = secretKey;
            _secretKeyEncode = secretKeyEncode;
            _issuer = issuer;
            _audience = audience;
            _expiresType = expiresType;
            _expiresValue = expiresValue;
        }

        public string GetToken(List<Claim> claims)
        {
            SecurityTokenDescriptor tokenDescriptor = GetTokenDescriptor(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            //SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            //string token = tokenHandler.WriteToken(securityToken);
            var token = tokenHandler.CreateEncodedJwt(tokenDescriptor);

            return token;
        }
        public Guid GenerateRefreshToken()
        {
            // Genera un nuevo GUID (Globally Unique Identifier)
            Guid refreshToken = Guid.NewGuid();

            //// Convierte el GUID en una cadena sin guiones
            //string refreshTokenString = refreshToken.ToString("N");

            //return refreshTokenString;
            return refreshToken;
        }
        public bool ValidateRefreshToken(string refreshToken)
        {
            // Realiza la validación del Refresh Token aquí
            // Puede incluir lógica para verificar si el token existe en el almacenamiento
            // y si no ha expirado
            // Devuelve true si el token es válido, de lo contrario, false

            // Ejemplo simple: consideramos que todos los tokens son válidos
            return true;
        }
        public bool VerifyRefreshToken(string refreshToken, string storedRefreshToken)
        {
            // Compara el Refresh Token proporcionado con el almacenado
            // Debe ser una comparación segura para evitar ataques de temporización
            // Devuelve true si coincide, de lo contrario, false

            return string.Equals(refreshToken, storedRefreshToken, StringComparison.Ordinal);
        }

        private SecurityTokenDescriptor GetTokenDescriptor(List<Claim> claims)
        {
            var getCredentials = GetSigningCredentials();
            var encryptingCredentials = GetEncryptingCredentials();

            var Expires = _expiresType == "days" ? DateTime.UtcNow.AddDays(_expiresValue) : _expiresType == "minutes" ? DateTime.UtcNow.AddMinutes(_expiresValue) : DateTime.UtcNow.AddDays(2);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _issuer,
                Audience = _audience,
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.Now,
                Expires = Expires,
                SigningCredentials = getCredentials,
                EncryptingCredentials = encryptingCredentials
            };

            return tokenDescriptor;
        }
        private EncryptingCredentials GetEncryptingCredentials()
        {
            var keyBytes = Encoding.UTF8.GetBytes(_secretKeyEncode);
            return new EncryptingCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512);
        }
        private SigningCredentials GetSigningCredentials()
        {
            var keyBytes = Encoding.UTF8.GetBytes(_secretKey);
            return new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature);
        }
        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var getCredentials = GetSigningCredentials();
            var encryptingCredentials = GetEncryptingCredentials();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = getCredentials.Key,
                TokenDecryptionKey = encryptingCredentials.Key,
                ValidateLifetime = true
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch (Exception ex)
            {
                // Create a StackTrace object
                var stackTrace = new StackTrace(ex, true);

                return null; // Token is invalid
            }
        }
    }
}

