using System;
using System.Collections.Generic;
using Auth.Domain.Entities;

namespace Auth.Domain.Interfaces
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

