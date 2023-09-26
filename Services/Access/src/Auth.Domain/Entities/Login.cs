#nullable enable
using System;

namespace Access.Auth.Domain.Entities
{
	public class Login
	{
        public  Guid enterprise_id { get; set; }
        public Guid module_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
    public class LoginSP
    {
        public Guid? ProfileID { get; set; }
        public Guid? MenuID { get; set; }
        public Guid? ParentID { get; set; }
        public int? Order { get; set; }
        public Guid? ModuleID { get; set; }
        public Guid? ResourceID { get; set; }
        public string? Access { get; set; }
        public string? Message { get; set; }
    }
    public class ProtectedDataSP
    {
        public Guid? ProfileID { get; set; }
        public Guid? TableName { get; set; }
        public string? Campos { get; set; }
        public string? Message { get; set; }
    }
}