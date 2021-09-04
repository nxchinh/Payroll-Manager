namespace Payroll_Manager.Utilities
{
    public static class SD
    {
        public enum Roles
        {
            Admin,
            Hr,
            Payroll,
            DepartmentHead,
            Worker
        }
        
        public const string Admin = "Admin";
        public const string Hr = "Hr";
        public const string Payroll = "Payroll";
        public const string DepartmentHead = "DepartmentHead";
        public const string Worker = "Worker";
    }
}
