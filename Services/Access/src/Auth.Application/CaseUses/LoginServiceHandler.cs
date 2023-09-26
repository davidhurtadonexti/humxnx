using System;
using System.Collections.Generic;
using Access.Auth.Domain.Entities;
using Access.Auth.Domain.Interfaces;

namespace Access.Auth.Application.CaseUses
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

