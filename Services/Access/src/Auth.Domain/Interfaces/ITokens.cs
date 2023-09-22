using System;
using System.Collections.Generic;
using Auth.Domain.Entities;

namespace Auth.Domain.Interfaces
{
	public interface ITokens
	{
        public List<Tokens> GetAll();
        public Tokens GetOne(Guid TokenID);
        public Tokens NewItem(Tokens Token);
        public Tokens UpdateItem(Guid TokenID, Tokens Token);
        public bool DeleteItem(Guid TokenID);
    }
}

