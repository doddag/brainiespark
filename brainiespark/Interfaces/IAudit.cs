using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace brainiespark.Interfaces
{
    internal interface IAudit
    {
        string EnteredBy { get; set; }
        DateTime DateTimeEntered { get; set; }
        string ModifiedBy { get; set; }
        DateTime DateTimeModified { get; set; }
        DateTime TimeStamp { get; set; }
    }
}
