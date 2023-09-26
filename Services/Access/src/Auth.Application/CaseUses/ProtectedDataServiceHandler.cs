using System;
using System.Collections.Generic;
using Access.Auth.Domain.Entities;
using Access.Auth.Domain.Interfaces;

namespace Access.Auth.Application.CaseUses
{
	public class ProtectedDataServiceHandler
	{
        private readonly IProtectedData _protectedData;
        public ProtectedDataServiceHandler(IProtectedData protectedData)
        {
            _protectedData = protectedData;
        }
        public ProtectedData CreateProtectedData(ProtectedData profile)
        {
            return _protectedData.NewItem(profile);
        }
        public ProtectedData UpdateProtectedData(Guid ProtectedDataID, ProtectedData profile)
        {
            return _protectedData.UpdateItem(ProtectedDataID, profile);
        }
        public bool DeleteProtectedData(Guid ProtectedDataID)
        {
            return _protectedData.DeleteItem(ProtectedDataID);
        }
        public List<ProtectedData> GetAllProtectedData()
        {
            return _protectedData.GetAll();
        }
        public ProtectedData GetOneProtectedData(Guid ProtectedDataID)
        {
            return _protectedData.GetOne(ProtectedDataID);
        }
    }
}

