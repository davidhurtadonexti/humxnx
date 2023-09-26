using System;
using System.Collections.Generic;
using Access.Auth.Domain.Entities;

namespace Access.Auth.Domain.Interfaces
{
	public interface IResources
	{
        public List<Resources> GetAll();
        public Resources GetOne(Guid ResourceID);
        public Resources NewItem(Resources Resource);
        public Resources UpdateItem(Guid ResourceID, Resources Resource);
        public bool DeleteItem(Guid ResourceID);
    }
}

