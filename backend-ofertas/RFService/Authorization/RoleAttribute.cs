namespace RFService.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class RoleAttribute(params string[] roles) : Attribute
    {
        public string[] Roles { get; set; } = roles;
    }
}
