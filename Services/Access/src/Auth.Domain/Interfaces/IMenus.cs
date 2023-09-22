using System;
using System.Collections.Generic;
using Auth.Domain.Entities;

namespace Auth.Domain.Interfaces
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

