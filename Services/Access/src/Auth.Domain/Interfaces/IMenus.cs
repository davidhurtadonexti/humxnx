using System;
using System.Collections.Generic;
using Access.Auth.Domain.Entities;

namespace Access.Auth.Domain.Interfaces
{
	public interface IMenus
	{
        public List<Menus> GetAll();
        public Menus GetOne(Guid MenuID);
        public Menus NewItem(Menus menu);
        public Menus UpdateItem(Guid MenuID, Menus menu);
        public bool DeleteItem(Guid MenuID);
    }
}

