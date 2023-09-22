using System;
using System.Collections.Generic;
using Auth.Domain.Entities;
using Auth.Domain.Interfaces;

namespace Auth.Application.CaseUses
{
	public class TokensServiceHandler
	{
        private readonly ITokens _tokens;
        public TokensServiceHandler(ITokens tokens)
		{
            _tokens = tokens;
        }
        public Tokens CreateToken(Tokens token)
        {
            return _tokens.NewItem(token);
        }
        public Tokens UpdateToken(Guid TokenID, Tokens token)
        {
            return _tokens.UpdateItem(TokenID, token);
        }
        public bool DeleteToken(Guid TokenID)
        {
            return _tokens.DeleteItem(TokenID);
        }
        public List<Tokens> GetAllToken()
        {
            return _tokens.GetAll();
        }
        public Tokens GetOneToken(Guid TokenID)
        {
            return _tokens.GetOne(TokenID);
        }
    }
}

