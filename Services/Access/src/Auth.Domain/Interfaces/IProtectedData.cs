using System;
using System.Collections.Generic;
using Auth.Domain.Entities;

namespace Auth.Domain.Interfaces
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

