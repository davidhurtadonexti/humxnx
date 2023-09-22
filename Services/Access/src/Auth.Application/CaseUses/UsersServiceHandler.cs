using System;
using System.Collections.Generic;
using Auth.Domain.Entities;
using Auth.Domain.Interfaces;

namespace Auth.Application.CaseUses
{
	public class UsersServiceHandler
	{
        private readonly IUsers _users;
        public UsersServiceHandler(IUsers users)
        {
            _users = users;
        }
        public Users CreateUser(Users user)
        {
            return _users.NewItem(user);
        }
        public Users UpdateUser(Guid UsersID, Users user)
        {
            return _users.UpdateItem(UsersID, user);
        }
        public bool DeleteUser(Guid UsersID)
        {
            return _users.DeleteItem(UsersID);
        }
        public List<Users> GetAllUser()
        {
            return _users.GetAll();
        }
        public Users GetOneUser(Guid UsersID)
        {
            return _users.GetOne(UsersID);
        }
        public Users GetOneByUsernameUser(string username)
        {
            return _users.GetOneByUserName(username);
        }
        public Users_Profiles LinkProfile(Users_Profiles Users_Profiles)
        {
            return _users.LinkProfile(Users_Profiles);
        }
        public Profile_ProtectedData LinkProtectData(Profile_ProtectedData Profile_ProtectedData)
        {
            return _users.LinkProtectData(Profile_ProtectedData);
        }
        public Profiles_Resources LinkResources(Profiles_Resources Profiles_Resources)
        {
            return _users.LinkResources(Profiles_Resources);
        }
        public Users_Profiles LinkProfileUpdate(Guid ItemID, Users_Profiles Users_Profiles)
        {
            return _users.LinkProfileUpdate(ItemID, Users_Profiles);
        }
        public Profile_ProtectedData LinkProtectDataUpdate(Guid ItemID, Profile_ProtectedData Profile_ProtectedData)
        {
            return _users.LinkProtectDataUpdate(ItemID, Profile_ProtectedData);
        }
        public Profiles_Resources LinkResourcesUpdate(Guid ItemID, Profiles_Resources Profiles_Resources)
        {
            return _users.LinkResourcesUpdate(ItemID, Profiles_Resources);
        }
    }
}