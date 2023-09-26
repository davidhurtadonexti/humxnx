using System;
using System.Collections.Generic;
using Access.Auth.Domain.Entities;
using Access.Auth.Domain.Interfaces;

namespace Access.Auth.Application.CaseUses
{
	public class ProfilesServiceHandler
	{
        private readonly IProfiles _profiles;
        public ProfilesServiceHandler(IProfiles profiles)
        {
            _profiles = profiles;
        }
        public Profiles CreateProfile(Profiles profile)
        {
            return _profiles.NewItem(profile);
        }
        public Profiles UpdateProfile(Guid ProfileID, Profiles profile)
        {
            return _profiles.UpdateItem(ProfileID, profile);
        }
        public bool DeleteProfile(Guid ProfileID)
        {
            return _profiles.DeleteItem(ProfileID);
        }
        public List<Profiles> GetAllProfile()
        {
            return _profiles.GetAll();
        }
        public Profiles GetOneProfile(Guid ProfileID)
        {
            return _profiles.GetOne(ProfileID);
        }
    }
}

