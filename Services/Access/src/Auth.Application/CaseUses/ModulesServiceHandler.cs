using System;
using System.Collections.Generic;
using Auth.Domain.Entities;
using Auth.Domain.Interfaces;

namespace Auth.Application.CaseUses
{
	public class ModulesServiceHandler
	{
        private readonly IModules _modules;
        public ModulesServiceHandler(IModules modules)
        {
            _modules = modules;
        }
        public Modules CreateModule(Modules module)
        {
            return _modules.NewItem(module);
        }
        public Modules UpdateModule(Guid ModuleID, Modules module)
        {
            return _modules.UpdateItem(ModuleID, module);
        }
        public bool DeleteModule(Guid ModuleID)
        {
            return _modules.DeleteItem(ModuleID);
        }
        public List<Modules> GetAllModule()
        {
            return _modules.GetAll();
        }
        public Modules GetOneModule(Guid ModuleID)
        {
            return _modules.GetOne(ModuleID);
        }
    }
}

