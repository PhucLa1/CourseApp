using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data
{
    internal interface IAuditable
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
        int CreatedBy { get; set; }
        int UpdatedBy { get; set; }
    }
}
