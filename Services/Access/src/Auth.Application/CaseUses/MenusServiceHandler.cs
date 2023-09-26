using System;
using System.Collections.Generic;
using Access.Auth.Domain.Entities;
using Access.Auth.Domain.Interfaces;

namespace Access.Auth.Application.CaseUses
{
	public class MenusServiceHandler
	{
        private readonly IMenus _menus;
        public MenusServiceHandler(IMenus menus)
        {
            _menus = menus;
        }
        public Menus CreateMenu(Menus menu)
        {
            return _menus.NewItem(menu);
        }
        public Menus UpdateMenu(Guid MenuID, Menus menu)
        {
            return _menus.UpdateItem(MenuID, menu);
        }
        public bool DeleteMenu(Guid MenuID)
        {
            return _menus.DeleteItem(MenuID);
        }
        public List<Menus> GetAllMenu()
        {
            return _menus.GetAll();
        }
        public Menus GetOneMenu(Guid MenuID)
        {
            return _menus.GetOne(MenuID);
        }
    }
}

