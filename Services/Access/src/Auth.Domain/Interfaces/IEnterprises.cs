using System;
using System.Collections.Generic;
using Auth.Domain.Entities;

namespace Auth.Domain.Interfaces
{
	public interface IEnterprises
	{
        public List<Enterprises> GetAll();
        public Enterprises GetOne(Guid EnterpriseID);
        public Enterprises NewItem(Enterprises enterprise);
        public Enterprises UpdateItem(Guid EnterpriseID, Enterprises enterprise);
        public bool DeleteItem(Guid EnterpriseID);
    }
}

