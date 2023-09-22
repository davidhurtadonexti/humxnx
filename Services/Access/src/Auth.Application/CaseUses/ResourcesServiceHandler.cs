using System;
using System.Collections.Generic;
using Auth.Domain.Entities;
using Auth.Domain.Interfaces;

namespace Auth.Application.CaseUses
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

