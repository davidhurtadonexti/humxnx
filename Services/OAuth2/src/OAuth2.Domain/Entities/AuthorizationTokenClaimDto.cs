namespace OAuth2.OAuth2.Domain.Entities;

public class AuthorizationTokenClaimDto
{
    public object AuthorizationToken { get; set; }
    public object AccessToken { get; set; }
}