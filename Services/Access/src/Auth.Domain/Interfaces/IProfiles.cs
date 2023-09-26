using System;
using System.Collections.Generic;
using Access.Auth.Domain.Entities;

namespace Access.Auth.Domain.Interfaces
{
	public interface IProfiles
	{
        public List<Profiles> GetAll();
        public Profiles GetOne(Guid ProfileID);
        public Profiles NewItem(Profiles profile);
        public Profiles UpdateItem(Guid ProfileID, Profiles profile);
        public bool DeleteItem(Guid ProfileID);
    }
}

