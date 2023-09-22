using System;

namespace OAuth2.src.OAuth2.Infraestructure.Contracts.GetToken
{
    public class ClaimDataReturn
    {
        public Guid Id { get; set; }
        public string ServiceDescription { get; set; }
        public string ResourceDescription { get; set; }
        public string ClientDescription { get; set; }
        public string ResourceUrl { get; set; }
        public string Scopes { get; set; }
        public int AccesTokenExp { get; set; }
        public int RefreshTokenExp { get; set; }
    }
}
