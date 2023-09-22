using System;
using System.Collections.Generic;
using Auth.Domain.Entities;
using Auth.Domain.Interfaces;

namespace Auth.Application.CaseUses
{
	public class EnterprisesServiceHandler
    {
        private readonly IEnterprises _enterprises;
        public EnterprisesServiceHandler(IEnterprises enterprises)
		{
			_enterprises = enterprises;
		}
		public Enterprises CreateEnterprise(Enterprises enterprise)
		{
            return _enterprises.NewItem(enterprise);
        }
        public Enterprises UpdateEnterprise(Guid EnterpriseID, Enterprises enterprise)
        {
            return _enterprises.UpdateItem(EnterpriseID, enterprise);
        }
        public bool DeleteEnterprise(Guid EnterpriseID)
        {
            return _enterprises.DeleteItem(EnterpriseID);
        }
        public List<Enterprises> GetAllEnterprise()
        {
            return _enterprises.GetAll();
        }
        public Enterprises GetOneEnterprise(Guid EnterpriseID)
        {
            return _enterprises.GetOne(EnterpriseID);
        }
    }
}

