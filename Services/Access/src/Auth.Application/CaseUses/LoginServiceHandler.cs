using System;
using System.Collections.Generic;
using Auth.Domain.Entities;
using Auth.Domain.Interfaces;

namespace Auth.Application.CaseUses
{
	public class LoginServiceHandler : ILogin
    {
        private readonly ILogin _login;
        public LoginServiceHandler(ILogin login)
        {
            _login = login;
        }
        public List<LoginSP> LoginUser(Login login)
        {
            return _login.GetUserDataLogin(login);
        }

        public List<LoginSP> GetUserDataLogin(Login login)
        {
            return _login.GetUserDataLogin(login);
        }

        public List<LoginSP> GetUserDataBytoken(Guid token_id, Guid module_id)
        {
            return _login.GetUserDataBytoken(token_id, module_id);
        }

        public List<ProtectedDataSP> GetProfileGrants(Guid token_id)
        {
            return _login.GetProfileGrants(token_id);
        }
    }
}

