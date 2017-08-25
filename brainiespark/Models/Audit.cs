using System;
using System.Collections.Generic;

namespace brainiespark.Models
{
    public class Audit
    {
        public int Id { get; set; }
        public DateTime AuditDateTime { get; set; }
        public string EnteredByUserId { get; set; }
        public string TableName { get; set; }
        public string Activity { get; set; }
        public IList<AuditData> Data { get; set; }
    }
}