using System;
using System.Collections.Generic;
using Access.Auth.Domain.Entities;

namespace Access.Auth.Domain.Interfaces
{
	public interface ILogin
	{
        public List<LoginSP> GetUserDataLogin(Login login);
        public List<LoginSP> GetUserDataBytoken(Guid token_id, Guid module_id);
        public List<ProtectedDataSP> GetProfileGrants(Guid token_id);
    }
}

