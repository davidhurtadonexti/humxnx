using System;
using System.Collections.Generic;
using Auth.Domain.Entities;

namespace Auth.Domain.Interfaces
{
	public interface IUsers
	{
        public List<Users> GetAll();
        public Users GetOneByUserName(string username);
        public Users GetOne(Guid UserID);
        public Users NewItem(Users User);
        public Users UpdateItem(Guid UserID, Users User);
        public bool DeleteItem(Guid UserID);
        public Users_Profiles LinkProfile(Users_Profiles Users_Profiles);
        public Profile_ProtectedData LinkProtectData(Profile_ProtectedData Profile_ProtectedData);
        public Profiles_Resources LinkResources(Profiles_Resources Profiles_Resources);
        public Profiles_Resources LinkResourcesUpdate(Guid ItemID, Profiles_Resources Profiles_Resources);
        public Profile_ProtectedData LinkProtectDataUpdate(Guid ItemID, Profile_ProtectedData Profile_ProtectedData);
        public Users_Profiles LinkProfileUpdate(Guid ItemID, Users_Profiles Users_Profiles);
    }
}

