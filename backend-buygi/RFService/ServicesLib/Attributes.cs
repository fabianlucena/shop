namespace RFService.ServicesLib
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoincrementAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ForeignAttribute(string Column, string ForeignTable, string ForeignColumn = "Id", string ForeignSchema = "dbo") : Attribute
    {
        public string Column { get; } = Column;
        public string ForeignTable { get; } = ForeignTable;
        public string ForeignColumn { get; } = ForeignColumn;
        public string ForeignSchema { get; } = ForeignSchema;
    }
}

