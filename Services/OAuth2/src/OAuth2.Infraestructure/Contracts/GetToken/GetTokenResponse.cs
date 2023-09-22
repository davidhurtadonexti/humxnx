namespace OAuth2.src.OAuth2.Infraestructure.Contracts.GetToken
{
    public class GetTokenResponse
    {
        public string AuthorizationToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpirationToken { get; set; }
    }
}
