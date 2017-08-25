using System;

namespace brainiespark.Models
{
    public class AuditData
    {
        public int Id { get; set; }
        public string ColumnName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}