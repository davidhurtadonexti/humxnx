using System;
using System.Collections.Generic;
using Access.Auth.Domain.Entities;

namespace Access.Auth.Domain.Interfaces
{
	public interface IModules
	{
        public List<Modules> GetAll();
        public Modules GetOne(Guid ModuleID);
        public Modules NewItem(Modules module);
        public Modules UpdateItem(Guid ModuleID, Modules module);
        public bool DeleteItem(Guid ModuleID);
    }
}

