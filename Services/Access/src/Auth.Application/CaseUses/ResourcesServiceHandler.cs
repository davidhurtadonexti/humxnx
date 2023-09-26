using System;
using System.Collections.Generic;
using Access.Auth.Domain.Entities;
using Access.Auth.Domain.Interfaces;

namespace Access.Auth.Application.CaseUses
{
	public class ResourcesServiceHandler
	{
        private readonly IResources _resources;
        public ResourcesServiceHandler(IResources resources)
        {
            _resources = resources;
        }
        public Resources CreateResource(Resources resource)
        {
            return _resources.NewItem(resource);
        }
        public Resources UpdateResource(Guid ResourcesID, Resources resource)
        {
            return _resources.UpdateItem(ResourcesID, resource);
        }
        public bool DeleteResource(Guid ResourcesID)
        {
            return _resources.DeleteItem(ResourcesID);
        }
        public List<Resources> GetAllResource()
        {
            return _resources.GetAll();
        }
        public Resources GetOneResource(Guid ResourcesID)
        {
            return _resources.GetOne(ResourcesID);
        }
    }
}

