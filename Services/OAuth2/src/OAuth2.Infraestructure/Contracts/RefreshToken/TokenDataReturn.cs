using System;

namespace OAuth2.src.OAuth2.Infraestructure.Contracts.RefreshToken
{
    public class TokenDataReturn
    {
        public string AuthorizationToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool wasUpdated { get; set; }
    }
}
