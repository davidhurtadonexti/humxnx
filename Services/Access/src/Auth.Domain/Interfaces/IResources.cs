using System;
using System.Collections.Generic;
using Auth.Domain.Entities;

namespace Auth.Domain.Interfaces
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

