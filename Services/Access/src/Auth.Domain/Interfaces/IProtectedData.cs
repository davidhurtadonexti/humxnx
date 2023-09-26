using System;
using System.Collections.Generic;
using Access.Auth.Domain.Entities;

namespace Access.Auth.Domain.Interfaces
{
	public interface IProtectedData
	{
        public List<ProtectedData> GetAll();
        public ProtectedData GetOne(Guid ProtectedDataID);
        public ProtectedData NewItem(ProtectedData ProtectedData);
        public ProtectedData UpdateItem(Guid ProtectedDataID, ProtectedData ProtectedData);
        public bool DeleteItem(Guid ProtectedDataID);
    }
}

