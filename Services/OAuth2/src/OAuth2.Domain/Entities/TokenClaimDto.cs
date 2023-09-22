using System;
using OAuth2.src.OAuth2.Infraestructure.Contracts.GetToken;

namespace OAuth2.OAuth2.Domain.Entities;

public class TokenClaimDto
{
    public Guid ClientServiceId { get; set; }
    public string ClientDescripcion { get; set; }
    public string ServiceDescription { get; set; }
    public int AccesTokenExp { get; set; }
    public int RefreshTokenExp { get; set; }
    public PermisionsDto[] Resources { get; set; }
}