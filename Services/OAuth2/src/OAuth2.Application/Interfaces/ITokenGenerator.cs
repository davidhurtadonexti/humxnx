namespace OAuth2.src.OAuth2.Application.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(string input, string type, int expiration);
    }
}
