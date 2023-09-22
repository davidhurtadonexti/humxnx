using System;
using System.Collections.Generic;
using Auth.Domain.Entities;

namespace Auth.Domain.Interfaces
{
	public interface ILogin
	{
        public List<LoginSP> GetUserDataLogin(Login login);
        public List<LoginSP> GetUserDataBytoken(Guid token_id, Guid module_id);
        public List<ProtectedDataSP> GetProfileGrants(Guid token_id);
    }
}

